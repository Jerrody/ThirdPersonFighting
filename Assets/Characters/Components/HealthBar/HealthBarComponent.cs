using UnityEngine;
using UnityEngine.UI;

namespace Characters.Components.HealthBar
{
    public sealed class HealthBarComponent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private Image foreground;

        [Header("Stats")]
        [SerializeField] private float reduceSpeed = 2.0f;

        private Transform _playerCameraTransform;

        private float _target = 1.0f;

        private void Awake()
        {
            _playerCameraTransform = GameObject.FindWithTag(Tags.Camera).transform;
            healthComponent.OnHealthChange += OnHealthChange;
        }

        private void OnHealthChange(float totalHealth, float currentHealthAmount)
        {
            _target = currentHealthAmount / totalHealth;
        }

        private void Update()
        {
            var currentTransform = transform;
            currentTransform.rotation =
                Quaternion.LookRotation(currentTransform.position - _playerCameraTransform.position);
            foreground.fillAmount = Mathf.MoveTowards(foreground.fillAmount, _target, reduceSpeed * Time.deltaTime);
        }
    }
}