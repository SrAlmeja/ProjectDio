//Daniel Navarrete 28/03/2023  Controller by events

using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class Listener : MonoBehaviour
    {
        [SerializeField] protected BoolEventChannelSO handBrakeEvent;
        [SerializeField] protected VoidEventChannelSO stopTimeEvent;
        [SerializeField] protected FloatEventChannelSO angleEvent;
        [SerializeField] protected FloatEventChannelSO ltEvent;
        [SerializeField] protected FloatEventChannelSO rtEvent;
        [SerializeField] protected FloatEventChannelSO rotateEvent;
        [SerializeField] protected VectorTwoEventChannelSO vector2InputEvent;
        [SerializeField] public VoidEventChannelSO impulseEvent;

        protected void OnEnable()
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

        protected void OnDisable()
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

        protected virtual void HandBrake(bool b)
        {
            //Debug.Log("Booleano de freno: " + b);
        }

        protected virtual void StopTime()
        {
            //Debug.Log("Booleano de parar el tiempo: " + b);
        }

        protected virtual void Angle(float f)
        {
            //Debug.Log("Flotante llantas: " + f);
        }

        protected virtual void Lt(float f)
        {
            //Debug.Log("Flotante Aceleraci�n: " + f);
        }

        protected virtual void Rt(float f)
        {
            //Debug.Log("Flotante Freno: " + f);
        }

        protected virtual void Rotate(float f)
        {
            //Debug.Log("Flotante angulo: " + f);
        }

        protected virtual void VecTwoInput(Vector2 v2)
        {
            Debug.Log("vector2 value: " + v2);
        }

        protected virtual void Impulse()
        {
            //Debug.Log("Se llama el evento");
        }
    }
}