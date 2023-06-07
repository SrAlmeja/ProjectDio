using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class CameraInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset CameraMoveInputActions;
        InputActionMap gameplayActionMap;

        InputAction CameraInputAction;

        [SerializeField] private VectorTwoEventChannelSO _vector2Camera;

        private void OnEnable()
        {
            CameraInputAction.Enable();
        }

        private void OnDisable()
        {
            CameraInputAction.Disable();
        }

        private void Awake()
        {
            gameplayActionMap = CameraMoveInputActions.FindActionMap("Camera");
            CameraInputAction = gameplayActionMap.FindAction("Movement");

            CameraInputAction.performed += MoveCamera;
            CameraInputAction.canceled += MoveCamera;

            CameraInputAction.performed += MoveCamera;
        }

        void MoveCamera(InputAction.CallbackContext context)
        {
            _vector2Camera?.RaiseEvent(context.ReadValue<Vector2>());
        }
    }
}