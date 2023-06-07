using Unity.Netcode;
using UnityEngine;


namespace com.LazyGames.Dio
{
    public class NetworkSteeringEventsListener : NetworkBehaviour
    {
        [SerializeField] protected BoolEventChannelSO handBrakeEvent;
        [SerializeField] protected VoidEventChannelSO stopTimeEvent;
        [SerializeField] protected FloatEventChannelSO angleEvent;
        [SerializeField] protected FloatEventChannelSO ltEvent;
        [SerializeField] protected FloatEventChannelSO rtEvent;
        [SerializeField] protected FloatEventChannelSO rotateEvent;
        [SerializeField] protected VectorTwoEventChannelSO vector2InputEvent;
        [SerializeField] public VoidEventChannelSO impulseEvent;
     
        [HideInInspector]public bool handBrake;
        [HideInInspector]public float angle;
        [HideInInspector]public float lT;
        [HideInInspector]public float rT; 
        [HideInInspector]public Vector2 vec2Input;
        
        public event System.Action DoImpulseEvent;
        public event System.Action DoStopTimeEvent;

        public override void OnNetworkSpawn()
        {
            handBrakeEvent.BoolEvent += HandBrake;
            stopTimeEvent.VoidEvent += StopTime;
            angleEvent.FloatEvent += Angle;
            ltEvent.FloatEvent += Lt;
            rtEvent.FloatEvent += Rt;
            rotateEvent.FloatEvent += Rotate;
            vector2InputEvent.Vector2Event += VecTwoInput;
            impulseEvent.VoidEvent += Impulse;
        }
        
        public override void OnNetworkDespawn()
        {
            handBrakeEvent.BoolEvent -= HandBrake;
            stopTimeEvent.VoidEvent -= StopTime;
            angleEvent.FloatEvent -= Angle;
            ltEvent.FloatEvent -= Lt;
            rtEvent.FloatEvent -= Rt;
            rotateEvent.FloatEvent -= Rotate;
            vector2InputEvent.Vector2Event -= VecTwoInput;
            impulseEvent.VoidEvent -= Impulse;
        }

        void Rotate(float f)
        {
            //Debug.Log("Flotante angulo: " + f);
        }
        void HandBrake(bool b)
        {
            handBrake = b;
        }

        void StopTime()
        {
            DoStopTimeEvent?.Invoke();
        }

        void Angle(float f)
        {
            angle = f;
        }

        void Lt(float f)
        {
            lT = f;
        }
        
        void Rt(float f)
        {
            rT = f;
        }

        void VecTwoInput(Vector2 v2)
        {
            vec2Input = v2;
        }
        
        void Impulse()
        {
            DoImpulseEvent?.Invoke();
        }
        
        
        
    }
}

