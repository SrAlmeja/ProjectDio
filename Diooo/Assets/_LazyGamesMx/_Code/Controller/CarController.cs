//Daniel Navarrete 28/03/2023  Controller by events

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class CarController : MonoBehaviour
    {
        public InputActionAsset inputActions;
        InputActionMap gameplayActionMap;

        InputAction handBrakeInputAction;
        InputAction steeringInputAction;
        InputAction accelerationInputAction;
        InputAction TimeStopInputAction;

        public BoolEventChannelSO HanbreakEvent;
        public BoolEventChannelSO StopTimeEvent;
        public FloatEventChannelSO AngleEvent;
        public FloatEventChannelSO TorqueEvent;

        void Awake()
        {
            gameplayActionMap = inputActions.FindActionMap("Gameplay");

            handBrakeInputAction = gameplayActionMap.FindAction("HandBrake");
            steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
            accelerationInputAction = gameplayActionMap.FindAction("Acceleration");
            TimeStopInputAction = gameplayActionMap.FindAction("TimeStop");

            handBrakeInputAction.performed += GetHandBrakeInput;
            handBrakeInputAction.canceled += GetHandBrakeInput;

            steeringInputAction.performed += GetAngleInput;
            steeringInputAction.canceled += GetAngleInput;

            accelerationInputAction.performed += GetTorqueInput;
            accelerationInputAction.canceled += GetTorqueInput;

            TimeStopInputAction.performed += StopTimeInput;
            TimeStopInputAction.canceled += StopTimeInput;
        }

        void GetHandBrakeInput(InputAction.CallbackContext context)
        {
            if(context.ReadValue<float>() == 0)
            {
                HanbreakEvent.RaiseEvent(false);
            }
            else
            {
                HanbreakEvent.RaiseEvent(true);
            }  
        }

        void GetAngleInput(InputAction.CallbackContext context)
        {
            AngleEvent.RaiseEvent(context.ReadValue<float>());
        }
        void GetTorqueInput(InputAction.CallbackContext context)
        {
            TorqueEvent.RaiseEvent(context.ReadValue<float>());
        }

        void StopTimeInput(InputAction.CallbackContext context) 
        {
            if (context.ReadValue<float>() == 0)
            {
                StopTimeEvent.RaiseEvent(false);
            }
            else
            {
                StopTimeEvent.RaiseEvent(true);
            }
        }

        private void OnEnable()
        {
            handBrakeInputAction.Enable();
            steeringInputAction.Enable();
            accelerationInputAction.Enable();
            TimeStopInputAction.Enable();
        }
        private void OnDisable()
        {
            handBrakeInputAction.Disable();
            steeringInputAction.Disable();
            accelerationInputAction.Disable();
            TimeStopInputAction.Disable();
        }
    }
}