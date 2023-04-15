//creado Raymundo "CryoStorage" Mosqueda 07/04/2023
//

using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class SteeringEventsWrapper : NetworkBehaviour
    {
        [Header("Unity Input References")]
        public InputActionAsset inputActions;
        
        [Header("EventChannelSo References")]
        [SerializeField]private BoolEventChannelSO handbrakeEvent;
        [SerializeField]private BoolEventChannelSO stopTimeEvent;
        [SerializeField]private FloatEventChannelSO angleEvent;
        [SerializeField]private FloatEventChannelSO torqueEvent;

        [SerializeField]private InputActionMap _gameplayActionMap;
        private InputAction _handBrakeInputAction;
        private InputAction _steeringInputAction;
        private InputAction _accelerationInputAction;
        private InputAction _timeStopInputAction;
        
        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            Debug.Log($"<color=green>SteeringEventsWrapper By{NetworkObject.OwnerClientId} Is Local PLAYER = {IsLocalPlayer} </color>" );
            
            Prepare();
            EnableInputs();
        }
        
        private void EnableInputs()
        {
        Debug.Log("<color=blue>SteeringEventsWrapper OnEnable INPUT ACTIONS</color>");
        _handBrakeInputAction.Enable();
        _steeringInputAction.Enable();
        _accelerationInputAction.Enable();
        _timeStopInputAction.Enable();
        }
        // private void OnDisable()
        // {
        //     if (!IsOwner)
        //     {
        //         return;
        //     }
        //     _handBrakeInputAction.Disable();
        //     _steeringInputAction.Disable();
        //     _accelerationInputAction.Disable();
        //     _timeStopInputAction.Disable();
        // }
        
        void GetHandBrakeInput(InputAction.CallbackContext context)
        {
            if(context.ReadValue<float>() == 0)
            {
                handbrakeEvent.RaiseEvent(false);
            }
            else
            {
                handbrakeEvent.RaiseEvent(true);
            }  
        }

        void GetAngleInput(InputAction.CallbackContext context)
        {
            angleEvent.RaiseEvent(context.ReadValue<float>());
        }
        void GetTorqueInput(InputAction.CallbackContext context)
        {
            torqueEvent.RaiseEvent(context.ReadValue<float>());
        }

        void StopTimeInput(InputAction.CallbackContext context) 
        {
            if (context.ReadValue<float>() == 0)
            {
                stopTimeEvent.RaiseEvent(false);
            }
            else
            {
                stopTimeEvent.RaiseEvent(true);
            }
        }

        private void Prepare()
        {
            try
            {
                _gameplayActionMap = inputActions.FindActionMap("Gameplay");
                _handBrakeInputAction = _gameplayActionMap.FindAction("HandBrake");
                _steeringInputAction = _gameplayActionMap.FindAction("SteeringAngle");
                _accelerationInputAction = _gameplayActionMap.FindAction("Acceleration");
                _timeStopInputAction = _gameplayActionMap.FindAction("TimeStop");
                
            }catch { Debug.LogWarning($"{gameObject.name} error preparing input actions"); }
            
            _handBrakeInputAction.performed += GetHandBrakeInput;
            _handBrakeInputAction.canceled += GetHandBrakeInput;

            _steeringInputAction.performed += GetAngleInput;
            _steeringInputAction.canceled += GetAngleInput;

            _accelerationInputAction.performed += GetTorqueInput;
            _accelerationInputAction.canceled += GetTorqueInput;

            _timeStopInputAction.performed += StopTimeInput;
            _timeStopInputAction.canceled += StopTimeInput;

        }
    }
}

