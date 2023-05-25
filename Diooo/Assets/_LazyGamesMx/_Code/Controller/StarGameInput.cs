using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class StarGameInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        InputActionMap gameplayActionMap;
        InputAction startGameInputAction;

        private void OnEnable()
        {
            gameplayActionMap = inputActions.FindActionMap("Start");
            startGameInputAction = gameplayActionMap.FindAction("GoToMatch");
            startGameInputAction.Enable();

            startGameInputAction.performed += OnStarGame;
            startGameInputAction.canceled += OnStarGame;
        }

        void OnStarGame(InputAction.CallbackContext context)
        {
            //Start Event
        }
    }
}