using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Scenes.MainMenu
{
    public sealed class PlayButtonController : MonoBehaviour
    {
        private void Awake()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void OnClick()
        {
            SceneManager.LoadScene(Levels.Main);

            InputSystem.EnableDevice(Keyboard.current);
        }
    }
}