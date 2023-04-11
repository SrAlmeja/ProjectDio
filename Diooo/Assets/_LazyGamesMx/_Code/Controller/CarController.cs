//Daniel Navarrete 28/03/2023  Controller by events for the Car

using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        InputActionMap gameplayActionMap;

        InputAction handBrakeInputAction;
        InputAction steeringInputAction;
        InputAction accelerationInputAction;
        InputAction TimeStopInputAction;
        InputAction RotateInputAction;

        [SerializeField] private BoolEventChannelSO HanbreakEvent;
        [SerializeField] private BoolEventChannelSO StopTimeEvent;
        [SerializeField] private FloatEventChannelSO AngleEvent;
        [SerializeField] private FloatEventChannelSO TorqueEvent;
        [SerializeField] private FloatEventChannelSO RotateEvent;

        private void OnEnable()
        {
            handBrakeInputAction.Enable();
            steeringInputAction.Enable();
            accelerationInputAction.Enable();
            TimeStopInputAction.Enable();
            RotateInputAction.Enable();
        }
        private void OnDisable()
        {
            handBrakeInputAction.Disable();
            steeringInputAction.Disable();
            accelerationInputAction.Disable();
            TimeStopInputAction.Disable();
            RotateInputAction.Disable();
        }

        void Awake()
        {
            gameplayActionMap = inputActions.FindActionMap("Gameplay");

            handBrakeInputAction = gameplayActionMap.FindAction("HandBrake");
            steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
            accelerationInputAction = gameplayActionMap.FindAction("Acceleration");
            TimeStopInputAction = gameplayActionMap.FindAction("TimeStop");
            RotateInputAction = gameplayActionMap.FindAction("Rotate");

            handBrakeInputAction.performed += GetHandBrakeInput;
            handBrakeInputAction.canceled += GetHandBrakeInput;

            steeringInputAction.performed += GetAngleInput;
            steeringInputAction.canceled += GetAngleInput;

            accelerationInputAction.performed += GetTorqueInput;
            accelerationInputAction.canceled += GetTorqueInput;

            TimeStopInputAction.performed += StopTimeInput;
            TimeStopInputAction.canceled += StopTimeInput;

            RotateInputAction.performed += RotateInput;
            RotateInputAction.canceled += RotateInput;
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

        void RotateInput(InputAction.CallbackContext context)
        {
            Vector2 VectorInput = context.ReadValue<Vector2>();
            float angle = Mathf.Atan2(VectorInput.y, VectorInput.x) * Mathf.Rad2Deg;

            if (angle < 0)
            {
                angle += 360f;
            }
            else if (angle > 360f)
            {
                angle -= 360f;
            }

            if (angle > 180f)
            {
                angle = 360f - angle;
            }

            RotateEvent.RaiseEvent(angle);
        }
    }
}