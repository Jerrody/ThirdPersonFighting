using Characters.Animation;
using Characters.Enemy;
using TMPro;
using UnityEngine;

namespace Characters.Player.UI
{
    public sealed class PlayerUIController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private PlayerController player;

        [SerializeField] private Camera playerCamera;
        [SerializeField] private AnimationEventListener playerAnimEvent;

        [Header("Canvases")] [SerializeField] private Canvas escape;

        [Header("Text")] [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI lossText;

        private uint _scores;

        private void Awake()
        {
            player.OnDeath += OnDeath;
            player.OnEscapePressed += OnEscapePressed;
            playerAnimEvent.OnDeathAnimationEnd += OnEscapePressed;
            EnemyController.OnKill += () => { _scores++; };

            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 1.0f;
        }

        private void OnEscapePressed()
        {
            var escapeGameObject = escape.gameObject;
            escapeGameObject.SetActive(!escapeGameObject.activeSelf);
            var isEscapeActive = escapeGameObject.activeSelf;

            playerCamera.gameObject.SetActive(!isEscapeActive);
            SetScoreText();
            scoreText.gameObject.SetActive(isEscapeActive);

            Cursor.visible = isEscapeActive;
            Cursor.lockState = isEscapeActive ? CursorLockMode.Confined : CursorLockMode.Locked;

            Time.timeScale = isEscapeActive ? 0.0f : 1.0f;
        }

        private void OnDeath()
        {
            lossText.gameObject.SetActive(true);
            SetScoreText();
            scoreText.gameObject.SetActive(true);
            
            OnEscapePressed();
        }

        private void SetScoreText()
        {
            scoreText.text = "Score: " + _scores;
        }
    }
}