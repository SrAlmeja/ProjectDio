using Unity.Netcode;
using UnityEngine;


namespace com.LazyGames.Dio
{
    public class NetworkSteeringEventsListener : NetworkBehaviour
    {
        [HideInInspector]public bool handBrake;
        [HideInInspector]public bool stopTime;
        [HideInInspector]public float angle;
        [HideInInspector]public float torque;
        [HideInInspector]public float rotate;
        [HideInInspector]public Vector2 Vec2Input;
        
        [SerializeField] protected BoolEventChannelSO _handBrakeEvent;
        [SerializeField] protected BoolEventChannelSO _stopTimeEvent;
        [SerializeField] protected FloatEventChannelSO _angleEvent;
        [SerializeField] protected FloatEventChannelSO _torqueEvent;
        [SerializeField] protected FloatEventChannelSO _rotateEvent;
        [SerializeField] protected VectorTwoEventChannelSO _vector2InputEvent;
        [SerializeField] protected VoidEventChannelSO _impulseEvent;

        
        private NetworkCarImpulse _carImpulse;

        public override void OnNetworkSpawn()
        {
            if(!IsOwner) return;
            
            DioGameManagerMultiplayer.Instance.OnGameStateChange += Prepare;
            // Prepare();
            base.OnNetworkSpawn();

        }

        private void HandBrake(bool b)
        {
            if(!IsOwner) return;
            handBrake = b;
        }

        private void StopTime(bool b)
        {
            if(!IsOwner) return;
            stopTime = b;
        }

        private void Angle(float f)
        {
            if(!IsOwner) return;
            angle = f;
        }

        private void Torque(float f)
        {
            if(!IsOwner) return;
            torque = f;
        }

        private void Rotate(float f)
        {
            if(!IsOwner) return;
            rotate = f;
        }

        private void VecTwoInput(Vector2 v2)
        {
            if(!IsOwner) return;
            Vec2Input = v2;
        }

        private void Impulse()
        {
            if(!IsOwner) return;
            _carImpulse.ApplyImpulse();
        }

        private void Prepare(DioGameManagerMultiplayer.GameStates state)
        {
            if(!IsOwner) return;
            _carImpulse = GetComponent<NetworkCarImpulse>();

            if (state == DioGameManagerMultiplayer.GameStates.GamePlaying)
            {
                Debug.Log("<color=#E982EF>Enable driving Inputs </color>");
                _handBrakeEvent.BoolEvent += HandBrake;
                _stopTimeEvent.BoolEvent += StopTime;
                _angleEvent.FloatEvent += Angle;
                _torqueEvent.FloatEvent += Torque;
                _rotateEvent.FloatEvent += Rotate;
                _vector2InputEvent.Vector2Event += VecTwoInput;
                _impulseEvent.VoidEvent += Impulse;
            }
            else if (state == DioGameManagerMultiplayer.GameStates.GameOver)
            {
                Debug.Log("<color=#E982EF>Disable driving Inputs </color>");

                _handBrakeEvent.BoolEvent -= HandBrake;
                _stopTimeEvent.BoolEvent -= StopTime;
                _angleEvent.FloatEvent -= Angle;
                _torqueEvent.FloatEvent -= Torque;
                _rotateEvent.FloatEvent -= Rotate;
                _vector2InputEvent.Vector2Event -= VecTwoInput;
                _impulseEvent.VoidEvent -= Impulse;    
            }
            
        }
    }
}

