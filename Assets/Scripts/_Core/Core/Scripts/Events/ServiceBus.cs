using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Utility;
using UnityEditor;
using UnityEngine;

namespace Core.Events
{
    public static class ServiceBus
    {
        private static readonly List<(object instance, string topic, int priority, MethodInfo method)> _subscribers =
            new List<(object instance, string topic, int priority, MethodInfo method)>();

        private static readonly string[] FUNCTIONS_TO_IGNORE = new string[]
            { "Start", "Update", "FixedUpdate", "Awake", "OnEnable", "OnDisable", "OnDestroy" };

        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        private static void RegisterDefaultReferences()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public |
                                                           BindingFlags.Static))
                    {
                        var attributes = method.GetCustomAttributes<TopicAttribute>();
                        if (!attributes.Any())
                            continue;
                        foreach (var attribute in attributes)
                        {
                            if (attribute == null || string.IsNullOrEmpty(attribute.TopicName))
                                continue;
                            _subscribers.Add((null, attribute.TopicName, attribute.Priority, method));
                        }
                    }
                }
            }
        }

        public static void RegisterInstance(object instance)
        {
            foreach (var methodSubscription in ParseObject(instance))
                _subscribers.Add((instance, methodSubscription.topic, methodSubscription.priority,
                    methodSubscription.method));
        }

        public static void UnregisterInstance(object instance)
        {
            _subscribers.RemoveAll(x => x.instance == instance);
        }

        public static void Publish(string topicName, object sender, params object[] args)
        {
            MainThreadDispatcher.Instance.enabled = true;
            PublishInternal(topicName, sender, args);
        }

        private static void PublishInternal(string topicName, object sender, params object[] args)
        {
            List<IEnumerator> funcs = new List<IEnumerator>();
            foreach (var (instance, priority, method) in GetSubscribersFor(topicName)
                         .OrderByDescending(sub => sub.priority))
            {
                var @params = method.GetParameters();
                List<object> arguments = new List<object> { sender };
                if (@params.Length < 1)
                {
                    Debug.LogWarning($"Invalid receiver for event {topicName}: {method.Name}");
                    continue;
                }

                if (@params.Length == 2 &&
                    @params[1].GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0)
                    arguments.Add(args);

                foreach (var param in @params.Skip(1))
                {
                    object val = args.FirstOrDefault(v => v.GetType() == param.ParameterType);
                    if (val == null)
                        val = args.FirstOrDefault(v => param.ParameterType.IsAssignableFrom(v.GetType()));
                    //args.ToList().Remove(val);

                    if (val == null)
                    {
                        Debug.LogWarning(
                            $"Could not resolve value for {param} whilst publishing {topicName} at {method.Name}");
                        continue;
                    }

                    arguments.Add(val);
                }

                try
                {
                    method.Invoke(instance, arguments.ToArray());
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"{ex.Message} - {topicName} - {method} | {instance} | {arguments.ToArray().Length}");
                }
            }
        }

        private static IEnumerable<(object instance, int priority, MethodInfo method)> GetSubscribersFor(
            string topicName)
        {
            ICollection<(object instance, int priority, MethodInfo method)> subscribers =
                new List<(object instance, int priority, MethodInfo method)>();

            foreach (var (instance, topic, priority, method) in _subscribers.Where(x => x.topic == topicName))
            {
                subscribers.Add((instance, priority, method));
            }

            return subscribers;
        }

        private static IEnumerable<(string topic, int priority, MethodInfo method)> ParseObject(object obj)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                if (FUNCTIONS_TO_IGNORE.Contains(method.Name))
                    continue;
                if (method.ReturnType != typeof(void) ||
                    method.Name.ToLower().StartsWith("network") ||
                    method.Name.ToLower().StartsWith("rpc"))
                    continue;

                var attributes = method.GetCustomAttributes<TopicAttribute>();
                if (!attributes.Any())
                    continue;
                foreach (var attribute in attributes)
                {
                    if (attribute == null || string.IsNullOrEmpty(attribute.TopicName))
                        continue;
                    yield return (attribute.TopicName, attribute.Priority, method);
                }
            }
        }
    }
}