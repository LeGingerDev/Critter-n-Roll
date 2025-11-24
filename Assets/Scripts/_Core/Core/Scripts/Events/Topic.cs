using System.Collections.Generic;
using System.Reflection;

namespace Core.Events
{
    public class Topic
    {
        public void Publish<T>(List<(object instance, MethodInfo method)> subscribers, T message)
        {
            foreach (var (instance, method) in subscribers)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(T))
                {
                    method.Invoke(instance, new object[] { message });
                }
            }
        }
    }
}