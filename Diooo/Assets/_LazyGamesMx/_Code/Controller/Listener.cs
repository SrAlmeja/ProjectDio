//Daniel Navarrete 28/03/2023  Controller by events

using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Listener : MonoBehaviour
    {
        public BoolEventChannelSO HanbreakeEvent;
        public BoolEventChannelSO StopTimeEvent;
        public FloatEventChannelSO AngleEvent;
        public FloatEventChannelSO TorqueEvent;

        private void OnEnable()
        {
            HanbreakeEvent.BoolEvent += HandBrake;
            StopTimeEvent.BoolEvent += StopTime;
            AngleEvent.FloatEvent += Angle;
            TorqueEvent.FloatEvent += Torque;
        }

        private void OnDisable()
        {
            HanbreakeEvent.BoolEvent -= HandBrake;
            StopTimeEvent.BoolEvent -= StopTime;
            AngleEvent.FloatEvent -= Angle;
            TorqueEvent.FloatEvent -= Torque;
        }

        void HandBrake(bool b)
        {
            Debug.Log("Valor booleano: " + b);
        }

        void StopTime(bool b)
        {
            Debug.Log("Valor booleano: " + b);
        }

        void Angle(float f)
        {
            Debug.Log("Flotante Uno: " + f);
        }

        void Torque(float f)
        {
            Debug.Log("Flotante Dos: " + f);
        }
    }
}