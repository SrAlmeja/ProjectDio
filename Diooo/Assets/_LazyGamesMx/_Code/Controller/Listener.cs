//Daniel Navarrete 28/03/2023  Controller by events

using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Listener : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO _hanbreakeEvent;
        [SerializeField] private BoolEventChannelSO _stopTimeEvent;
        [SerializeField] private FloatEventChannelSO _angleEvent;
        [SerializeField] private FloatEventChannelSO _torqueEvent;
        [SerializeField] private FloatEventChannelSO _rotateEvent;
        [SerializeField] private VectorTwoEventChannelSO _vector2InputEvent;
        [SerializeField] private VoidEventChannelSO _impulseEvent;

        private void OnEnable()
        {
            _hanbreakeEvent.BoolEvent += HandBrake;
            _stopTimeEvent.BoolEvent += StopTime;
            _angleEvent.FloatEvent += Angle;
            _torqueEvent.FloatEvent += Torque;
            _rotateEvent.FloatEvent += Rotate;
            _vector2InputEvent.Vector2Event += VecTwoInput;
            _impulseEvent.VoidEvent += Impulse;
        }

        private void OnDisable()
        {
            _hanbreakeEvent.BoolEvent -= HandBrake;
            _stopTimeEvent.BoolEvent -= StopTime;
            _angleEvent.FloatEvent -= Angle;
            _torqueEvent.FloatEvent -= Torque;
            _rotateEvent.FloatEvent -= Rotate;
            _vector2InputEvent.Vector2Event -= VecTwoInput;
            _impulseEvent.VoidEvent -= Impulse;
        }

        void HandBrake(bool b)
        {
            //Debug.Log("Booleano de freno: " + b);
        }

        void StopTime(bool b)
        {
            //Debug.Log("Booleano de parar el tiempo: " + b);
        }

        void Angle(float f)
        {
            //Debug.Log("Flotante llantas: " + f);
        }

        void Torque(float f)
        {
            //Debug.Log("Flotante Aceleración: " + f);
        }

        void Rotate(float f)
        {
            //Debug.Log("Flotante angulo: " + f);
        }

        void VecTwoInput(Vector2 v2)
        {
            Debug.Log("vector2 value: " + v2);
        }

        void Impulse()
        {
            //Debug.Log("Se llama el evento");
        }
    }
}