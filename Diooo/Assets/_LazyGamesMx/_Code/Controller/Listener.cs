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
        [SerializeField] private BoolEventChannelSO _impulseEvent;

        private void OnEnable()
        {
            _hanbreakeEvent.BoolEvent += HandBrake;
            _stopTimeEvent.BoolEvent += StopTime;
            _angleEvent.FloatEvent += Angle;
            _torqueEvent.FloatEvent += Torque;
            _rotateEvent.FloatEvent += Rotate;
            _impulseEvent.BoolEvent += Impulse;
        }

        private void OnDisable()
        {
            _hanbreakeEvent.BoolEvent -= HandBrake;
            _stopTimeEvent.BoolEvent -= StopTime;
            _angleEvent.FloatEvent -= Angle;
            _torqueEvent.FloatEvent -= Torque;
            _rotateEvent.FloatEvent -= Rotate;
            _impulseEvent.BoolEvent -= Impulse;
        }

        void HandBrake(bool b)
        {
            //Debug.Log("Valor booleano: " + b);
        }

        void StopTime(bool b)
        {
            //Debug.Log("Valor booleano: " + b);
        }

        void Angle(float f)
        {
            //Debug.Log("Flotante Uno: " + f);
        }

        void Torque(float f)
        {
            //Debug.Log("Flotante Dos: " + f);
        }

        void Rotate(float f)
        {
            Debug.Log("Angulo: " + f);
        }

        void Impulse(bool b)
        {
            Debug.Log("Valor booleano: " + b);
        }
    }
}