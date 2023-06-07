using Unity.Netcode;
using UnityEngine;


namespace com.LazyGames.Dio
{
    public class NetworkSteeringEventsListener : NetworkBehaviour
    {
        
        [HideInInspector]public bool handBrake;
        [HideInInspector]public float angle;
        [HideInInspector]public float torque; 
        [HideInInspector]public Vector2 vec2Input;
        
        public event System.Action DoImpulseEvent;
        public event System.Action DoStopTimeEvent;
        
        
        [SerializeField] protected BoolEventChannelSO _handBrakeEvent;
        [SerializeField] protected VoidEventChannelSO _stopTimeEvent;
        [SerializeField] protected FloatEventChannelSO _angleEvent;
        [SerializeField] protected FloatEventChannelSO _torqueEvent;
        [SerializeField] protected FloatEventChannelSO _rotateEvent;
        [SerializeField] protected VectorTwoEventChannelSO _vector2InputEvent;
        [SerializeField] public VoidEventChannelSO _impulseEvent;


        private NetworkCarImpulse _carImpulse;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            DioGameManagerMultiplayer.Instance.OnGameStateChange += Prepare;

            base.OnNetworkSpawn();

        }
        protected void HandBrake(bool b)
        {
            handBrake = b;
        }

        protected void StopTime()
        {
            DoStopTimeEvent?.Invoke();
        }

        protected void Angle(float f)
        {
            angle = f;
        }

        protected void Torque(float f)
        {
            torque = f;
        }
        
        protected void VecTwoInput(Vector2 v2)
        {
            vec2Input = v2;
        }
        
        protected void Impulse()
        {
            DoImpulseEvent?.Invoke();
        }
       

        private void Prepare(DioGameManagerMultiplayer.GameStates state)
        {
            if (!IsOwner) return;
            _carImpulse = GetComponent<NetworkCarImpulse>();

                if (state == DioGameManagerMultiplayer.GameStates.GamePlaying)
                {
                    Debug.Log("<color=#E982EF>Enable driving Inputs </color>");
                    _handBrakeEvent.BoolEvent += HandBrake;
                    // _stopTimeEvent.VoidEvent += StopTime;
                    _angleEvent.FloatEvent += Angle;
                    _torqueEvent.FloatEvent += Torque;
                    // _rotateEvent.FloatEvent += Rotate;
                    _vector2InputEvent.Vector2Event += VecTwoInput;
                    _impulseEvent.VoidEvent += Impulse;
                }
                else if (state == DioGameManagerMultiplayer.GameStates.GameOver)
                {
                    Debug.Log("<color=#E982EF>Disable driving Inputs </color>");
            
                    _handBrakeEvent.BoolEvent -= HandBrake;
                    // _stopTimeEvent.VoidEvent -= StopTime;
                    _angleEvent.FloatEvent -= Angle;
                    _torqueEvent.FloatEvent -= Torque;
                    // _rotateEvent.FloatEvent -= Rotate;
                    _vector2InputEvent.Vector2Event -= VecTwoInput;
                    _impulseEvent.VoidEvent -= Impulse;    
                }
        } 
    }
}

