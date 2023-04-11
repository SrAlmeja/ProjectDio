// Creado Raymundo "CryoStorage" Mosqueda 07/04/2023
//
using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class SteeringEventsListener : MonoBehaviour
    {
        [SerializeField]private BoolEventChannelSO handbrakeEvent;
        [SerializeField]private BoolEventChannelSO stopTimeEvent;
        [SerializeField]private FloatEventChannelSO angleEvent;
        [SerializeField]private FloatEventChannelSO torqueEvent;

        [HideInInspector]public bool handBrake;
        [HideInInspector]public bool stopTime;
        [HideInInspector]public float angle;
        [HideInInspector]public float torque;
        private void OnEnable()
        {
            handbrakeEvent.BoolEvent += HandBrake;
            stopTimeEvent.BoolEvent += StopTime;
            angleEvent.FloatEvent += Angle;
            torqueEvent.FloatEvent += Torque;
        }

        private void OnDisable()
        {
            handbrakeEvent.BoolEvent -= HandBrake;
            stopTimeEvent.BoolEvent -= StopTime;
            angleEvent.FloatEvent -= Angle;
            torqueEvent.FloatEvent -= Torque;
        }

        private void HandBrake(bool b)
        {
            handBrake = b;
        }

        private void StopTime(bool b)
        {
            stopTime = b;
        }

        private void Angle(float f)
        {
            angle = f;
        }

        private void Torque(float f)
        {
            torque = f;
        }
    }
}
