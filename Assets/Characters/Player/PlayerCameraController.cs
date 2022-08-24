using UnityEngine;

namespace Characters.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform lookAt;

        // TODO: Refactor and clear code.
        private void LateUpdate()
        {
            var position = transform.position;
            var lookAtPosition = lookAt.position;

            var dirToLookAt = lookAtPosition - new Vector3(position.x, lookAtPosition.y, position.z);

            var controllerTransform = controller.transform;
            controllerTransform.forward = dirToLookAt.normalized;
            controllerTransform.Rotate(controllerTransform.forward * Time.deltaTime);
        }
    }
}