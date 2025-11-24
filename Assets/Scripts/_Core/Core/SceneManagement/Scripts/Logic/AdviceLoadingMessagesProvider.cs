using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement.Loading
{
    public class AdviceLoadingMessagesProvider : MonoBehaviour
    {
        public List<string> messages = new List<string>();

        public string GetRandomMessage()
        {
            if (messages.Count == 0)
                return string.Empty;
            return messages[Random.Range(0, messages.Count)];
        }
    }
}