using UnityEngine;
using Utilities.PropertyAttributes;

namespace SceneManagement.Logic
{
    public class GoToLevel : MonoBehaviour
    {
        [SerializeField, SceneDropdown] private string _levelToGoTo;

        public void LoadLevel()
        {
            SceneManager.Instance.GoToLevel(_levelToGoTo);
        }
    }
}