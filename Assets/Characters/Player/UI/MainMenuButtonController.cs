using UnityEngine;
using UnityEngine.SceneManagement;

namespace Characters.Player.UI
{
    public sealed class MainMenuButtonController : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene(Levels.MainMenu);
        }
    }
}