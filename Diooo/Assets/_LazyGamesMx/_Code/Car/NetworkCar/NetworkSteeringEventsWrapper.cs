using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

    namespace com.LazyGames.Dio 
    {
        public class NetworkSteeringEventsWrapper : NetworkBehaviour
        {
            [SerializeField] private InputActionAsset inputActions;
            InputActionMap gameplayActionMap;

            InputAction handBrakeInputAction;
            InputAction steeringInputAction;
            InputAction accelerationInputAction;
            InputAction timeStopInputAction;
            InputAction rotateInputAction;
            InputAction impulseInputAction;

            [SerializeField] private BoolEventChannelSO _hanbreakEvent;
            [SerializeField] private BoolEventChannelSO _stopTimeEvent;
            [SerializeField] private FloatEventChannelSO _angleEvent;
            [SerializeField] private FloatEventChannelSO _torqueEvent;
            [SerializeField] private FloatEventChannelSO _rotateEvent;
            [SerializeField] private VectorTwoEventChannelSO _vector2InputEvent;
            [SerializeField] private VoidEventChannelSO _impulseEvent;

            private float _tap = 0;

            public override void OnNetworkSpawn()
            {
                if (!IsOwner) return;
                handBrakeInputAction.Enable();
                steeringInputAction.Enable();
                accelerationInputAction.Enable();
                timeStopInputAction.Enable();
                rotateInputAction.Enable();
                impulseInputAction.Enable();
                
                Prepare();
                base.OnNetworkDespawn();
            }

            public override void OnNetworkDespawn()
            {
                if (!IsOwner) return;
                handBrakeInputAction.Disable();
                steeringInputAction.Disable();
                accelerationInputAction.Disable();
                timeStopInputAction.Disable();
                rotateInputAction.Disable();
                impulseInputAction.Disable();
                base.OnNetworkDespawn();
            }

            void Prepare()
            {
                if (!IsOwner) return;
                gameplayActionMap = inputActions.FindActionMap("Gameplay");

                handBrakeInputAction = gameplayActionMap.FindAction("HandBrake");
                steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
                accelerationInputAction = gameplayActionMap.FindAction("Acceleration");
                timeStopInputAction = gameplayActionMap.FindAction("TimeStop");
                rotateInputAction = gameplayActionMap.FindAction("Rotate");
                impulseInputAction = gameplayActionMap.FindAction("Impulse");

                handBrakeInputAction.performed += GetHandBrakeInput;
                handBrakeInputAction.canceled += GetHandBrakeInput;

                steeringInputAction.performed += GetAngleInput;
                steeringInputAction.canceled += GetAngleInput;

                accelerationInputAction.performed += GetTorqueInput;
                accelerationInputAction.canceled += GetTorqueInput;

                timeStopInputAction.performed += StopTimeInput;
                timeStopInputAction.canceled += StopTimeInput;

                rotateInputAction.performed += RotateInput;
                rotateInputAction.canceled += RotateInput;

                impulseInputAction.performed += GetImpulseInput;
                impulseInputAction.canceled += GetImpulseInput;
            }

            void GetHandBrakeInput(InputAction.CallbackContext context)
            {
                if (!IsOwner) return;
                if(context.ReadValue<float>() == 0)
                {
                    _hanbreakEvent.RaiseEvent(false);
                }
                else
                {
                    _hanbreakEvent.RaiseEvent(true);
                }  
            }

            void GetAngleInput(InputAction.CallbackContext context)
            {
                if (!IsOwner) return;
                _angleEvent.RaiseEvent(context.ReadValue<float>());
            }
            void GetTorqueInput(InputAction.CallbackContext context)
            {
                if (!IsOwner) return;
                _torqueEvent.RaiseEvent(context.ReadValue<float>());
            }

            void StopTimeInput(InputAction.CallbackContext context) 
            {
                if (!IsOwner) return;
                if(_tap == 0)
                {
                    _stopTimeEvent.RaiseEvent(true);
                    _tap = 1;
                    return;
                }
                if(_tap == 1)
                {
                    _stopTimeEvent.RaiseEvent(false);
                    _tap = 0;
                    return;
                }
            }

            void RotateInput(InputAction.CallbackContext context)
            {
                if (!IsOwner) return;
                Vector2 VectorInput = context.ReadValue<Vector2>();
                float angle = Vector2.Angle(Vector2.right, VectorInput); // obtiene el ángulo en un rango de 0 a 180 grados

                if (VectorInput.y < 0) // ajusta el ángulo si está en el segundo o tercer cuadrante
                {
                    angle = 360 - angle;
                }

                float finalAngle = Mathf.Atan2(VectorInput.y, VectorInput.x) * Mathf.Rad2Deg; // convierte el ángulo a grados

                if (finalAngle < 0) // ajusta el ángulo si es negativo
                {
                    finalAngle += 360;
                }

                _rotateEvent.RaiseEvent(finalAngle);
                _vector2InputEvent.RaiseEvent(context.ReadValue<Vector2>());
            }

            void GetImpulseInput(InputAction.CallbackContext context)
            {
                if (!IsOwner) return;
                _impulseEvent.RaiseEvent();
            }
        }
    }