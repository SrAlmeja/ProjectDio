using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class StartGameController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        InputActionMap gameplayActionMap;

        InputAction startGameInputAction;

        [SerializeField] private BoolEventChannelSO _playerReady;

        private float _tap = 0;

        private void OnEnable()
        {
            startGameInputAction.Enable();
        }

        private void OnDisable()
        {
            startGameInputAction.Disable();
        }

        private void Awake()
        {
            gameplayActionMap = inputActions.FindActionMap("Start");

            startGameInputAction = gameplayActionMap.FindAction("StartGame");

            startGameInputAction.performed += StartGame;
            startGameInputAction.canceled += StartGame;
        }

        void StartGame(InputAction.CallbackContext context)
        {
            if (_tap == 0)
            {
                _playerReady.RaiseEvent(true);
                _tap = 1;
                return;
            }
            if (_tap == 1)
            {
                _playerReady.RaiseEvent(false);
                _tap = 0;
                return;
            }
        }
    }
}

