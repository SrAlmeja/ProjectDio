//Daniel Navarrete 28/03/2023  Controller by events for UI

using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        InputActionMap gameplayActionMap;

        InputAction UpUIInputAction;
        InputAction DownUIInputAction;
        InputAction LeftUIInputAction;
        InputAction RightUIInputAction;
        InputAction SelectUIInputAction;
        InputAction BackUIInputAction;

        [SerializeField] private VoidEventChannelSO _upUIEvent;
        [SerializeField] private VoidEventChannelSO _downUIEvent;
        [SerializeField] private VoidEventChannelSO _leftUIEvent;
        [SerializeField] private VoidEventChannelSO _rightUIEvent;
        [SerializeField] private VoidEventChannelSO _selectUIEvent;
        [SerializeField] private VoidEventChannelSO _backUIEvent;

        private void OnEnable()
        {
            UpUIInputAction.Enable();
            DownUIInputAction.Enable();
            LeftUIInputAction.Enable();
            RightUIInputAction.Enable();
            SelectUIInputAction.Enable();
            BackUIInputAction.Enable();
        }

        private void OnDisable()
        {
            UpUIInputAction.Disable();
            DownUIInputAction.Disable();
            LeftUIInputAction.Disable();
            RightUIInputAction.Disable();
            SelectUIInputAction.Disable();
            BackUIInputAction.Disable();
        }

        private void Awake()
        {
            gameplayActionMap = inputActions.FindActionMap("UI");

            UpUIInputAction = gameplayActionMap.FindAction("UpUI");
            DownUIInputAction = gameplayActionMap.FindAction("DownUI");
            LeftUIInputAction = gameplayActionMap.FindAction("LeftUI");
            RightUIInputAction = gameplayActionMap.FindAction("RightUI");
            SelectUIInputAction = gameplayActionMap.FindAction("SelectUI");
            BackUIInputAction = gameplayActionMap.FindAction("BackUI");

            UpUIInputAction.performed += GetUpUIInput;
            UpUIInputAction.canceled += GetUpUIInput;

            DownUIInputAction.performed += GetDownUIInput;
            DownUIInputAction.canceled += GetDownUIInput;

            LeftUIInputAction.performed += GetLeftUIInput;
            LeftUIInputAction.canceled += GetLeftUIInput;

            RightUIInputAction.performed += GetRightUIInput;
            RightUIInputAction.canceled += GetRightUIInput;

            SelectUIInputAction.performed += GetSelectUIInput;
            SelectUIInputAction.canceled += GetSelectUIInput;

            BackUIInputAction.performed += GetBackUIInput;
            BackUIInputAction.canceled += GetBackUIInput;
        }

        void GetUpUIInput(InputAction.CallbackContext context)
        {
            _upUIEvent.RaiseEvent();
        }

        void GetDownUIInput(InputAction.CallbackContext context)
        {
            _downUIEvent.RaiseEvent();
        }

        void GetLeftUIInput(InputAction.CallbackContext context)
        {
            _leftUIEvent.RaiseEvent();
        }

        void GetRightUIInput(InputAction.CallbackContext context)
        {
            _rightUIEvent.RaiseEvent();
        }

        void GetSelectUIInput(InputAction.CallbackContext context)
        {
            _selectUIEvent.RaiseEvent();
        }

        void GetBackUIInput(InputAction.CallbackContext context)
        {
            _backUIEvent.RaiseEvent();
        }
    }
}