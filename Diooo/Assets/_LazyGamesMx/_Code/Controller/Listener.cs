//Daniel Navarrete 28/03/2023  Controller by events

using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class Listener : MonoBehaviour
    {
        [SerializeField] protected BoolEventChannelSO _handBrakeEvent;
        [SerializeField] protected BoolEventChannelSO _stopTimeEvent;
        [SerializeField] protected FloatEventChannelSO _angleEvent;
        [SerializeField] protected FloatEventChannelSO _torqueEvent;
        [SerializeField] protected FloatEventChannelSO _rotateEvent;
        [SerializeField] protected VectorTwoEventChannelSO _vector2InputEvent;
        [SerializeField] protected VoidEventChannelSO _impulseEvent;

        protected void OnEnable()
        {
            _handBrakeEvent.BoolEvent += HandBrake;
            _stopTimeEvent.BoolEvent += StopTime;
            _angleEvent.FloatEvent += Angle;
            _torqueEvent.FloatEvent += Torque;
            _rotateEvent.FloatEvent += Rotate;
            _vector2InputEvent.Vector2Event += VecTwoInput;
            _impulseEvent.VoidEvent += Impulse;
        }

        protected void OnDisable()
        {
            _handBrakeEvent.BoolEvent -= HandBrake;
            _stopTimeEvent.BoolEvent -= StopTime;
            _angleEvent.FloatEvent -= Angle;
            _torqueEvent.FloatEvent -= Torque;
            _rotateEvent.FloatEvent -= Rotate;
            _vector2InputEvent.Vector2Event -= VecTwoInput;
            _impulseEvent.VoidEvent -= Impulse;
        }

        protected virtual void HandBrake(bool b)
        {
            //Debug.Log("Booleano de freno: " + b);
        }

        protected virtual void StopTime(bool b)
        {
            //Debug.Log("Booleano de parar el tiempo: " + b);
        }

        protected virtual void Angle(float f)
        {
            //Debug.Log("Flotante llantas: " + f);
        }

        protected virtual void Torque(float f)
        {
            //Debug.Log("Flotante Aceleración: " + f);
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