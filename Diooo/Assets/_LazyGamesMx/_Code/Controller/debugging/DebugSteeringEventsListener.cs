// Creado Raymundo "CryoStorage" Mosqueda 24/04/2023
//
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DebugSteeringEventsListener : Listener
    {
        [HideInInspector]public bool handBrake;
        [HideInInspector]public bool stopTime;
        [HideInInspector]public float angle;
        [HideInInspector]public float torque;
        [HideInInspector]public float rotate;
        [HideInInspector]public Vector2 Vec2Input;
        
        public event System.Action _doImpulseEvent;
        
        private CarImpulse _carImpulse;

        private void Start()
        {
            Prepare();
        }

        protected override void HandBrake(bool b)
        {
            handBrake = b;
        }

        protected override void StopTime(bool b)
        {
            stopTime = b;
        }

        protected override void Angle(float f)
        {
            angle = f;
        }

        protected override void Torque(float f)
        {
            torque = f;
        }

        protected override void Rotate(float f)
        {
            rotate = f;
        }

        protected override void VecTwoInput(Vector2 v2)
        {
            Vec2Input = v2;
        }

        private void Prepare()
        {
            _carImpulse = GetComponent<CarImpulse>();
        }

        protected override void Impulse()
        {
            _doImpulseEvent?.Invoke();
        }
    }
}

