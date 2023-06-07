// Creado Raymundo "CryoStorage" Mosqueda 24/04/2023
//
using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class DebugSteeringEventsListener : Listener
    {
        [HideInInspector]public bool handBrake;
        [HideInInspector]public float angle;
        [HideInInspector]public float lT;
        [HideInInspector]public float rT; 
        [HideInInspector]public Vector2 vec2Input;
        
        public event System.Action DoImpulseEvent;
        public event System.Action DoStopTimeEvent;
        

        protected override void HandBrake(bool b)
        {
            handBrake = b;
        }

        protected override void StopTime()
        {
            DoStopTimeEvent?.Invoke();
        }

        protected override void Angle(float f)
        {
            angle = f;
        }

        protected override void Lt(float f)
        {
            lT = f;
        }

        protected override void Rt(float f)
        {
            rT = f;
        }

        protected override void VecTwoInput(Vector2 v2)
        {
            vec2Input = v2;
        }
        
        protected override void Impulse()
        {
            DoImpulseEvent?.Invoke();
        }
    }
}

