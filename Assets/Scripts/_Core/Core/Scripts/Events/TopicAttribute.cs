using System;

namespace Core.Events
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TopicAttribute : Attribute
    {
        public string TopicName { get; }
        public int Priority { get; }

        public TopicAttribute(string topicName, int priority = 1)
        {
            TopicName = topicName;
            Priority = priority;
        }
    }
}