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
        
        private NetworkCarImpulse _carImpulse;

        public override void OnNetworkSpawn()
        {
            if(!IsOwner) return;
            Prepare();
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

        private void Prepare()
        {
            if(!IsOwner) return;
            _carImpulse = GetComponent<NetworkCarImpulse>();
        }
    }
}

