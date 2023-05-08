//Dino 03/05/2023 Change script to a scriptable object to use it as a event channel
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Ready Event Input")]
    public class ReadyPlayerInput : ScriptableObject
    {
        [SerializeField] private InputActionAsset inputActions;
        InputActionMap gameplayActionMap;

        InputAction startGameInputAction;

        public Action OnPlayerReadyInput;
        
        private void OnEnable()
        {
            PrepareInputs();
        }
        private void PrepareInputs()
        {
            gameplayActionMap = inputActions.FindActionMap("Start");
            startGameInputAction = gameplayActionMap.FindAction("StartGame");
            startGameInputAction.Enable();

            startGameInputAction.performed += OnPressReady;
            startGameInputAction.canceled += OnPressReady;
        }

        void OnPressReady(InputAction.CallbackContext context)
        {
            Debug.Log("<color=#C2FF70>Ready Input</color>");
            OnPlayerReadyInput?.Invoke();
        }
    }
}

