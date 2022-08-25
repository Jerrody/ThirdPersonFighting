using UnityEngine;

namespace Characters.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform lookAt;

        private void LateUpdate()
        {
            var position = transform.position;
            var lookAtPosition = lookAt.position;

            var dirToLookAt = lookAtPosition - new Vector3(position.x, lookAtPosition.y, position.z);

            var controllerTransform = player.transform;
            controllerTransform.forward = dirToLookAt.normalized;
            controllerTransform.Rotate(controllerTransform.forward * Time.deltaTime);
        }
    }
}