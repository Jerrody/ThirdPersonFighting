using UnityEngine;

namespace Utils.UI
{
    public sealed class ExitButtonController : MonoBehaviour
    {
        public void OnClick()
        {
            Application.Quit();
        }
    }
}