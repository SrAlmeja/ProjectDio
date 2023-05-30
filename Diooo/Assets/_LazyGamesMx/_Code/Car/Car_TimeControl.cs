//Raymundo cryoStorage Mosqueda 07/03/2023
//
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class Car_TimeControl : MonoBehaviour
    {
        [Header("Time Control")] 
        [SerializeField] private AnimationCurve targetTimeScale;
        
        private bool doSlow;
        private DebugSteeringEventsListener _listener;
        private float savedMagnitude;
        private float currentTimeScale = 1;
        private readonly float normalizeFactor = .02f;
        private float _stasisMeter = 0f;
        [SerializeField]private float stasisDelta = .3f;

        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            Slow();
            doSlow = _listener.stopTime;
            Time.timeScale = currentTimeScale;
        }

        void Slow()
        {
            switch (doSlow && _listener.stopTime)
            {
                case true:
                    currentTimeScale = targetTimeScale.Evaluate(_stasisMeter);
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = false;
                    _stasisMeter -= stasisDelta * Time.deltaTime;
                    break;
                case false:
                    currentTimeScale = 1f;
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = true;
                    _stasisMeter += stasisDelta * Time.deltaTime;
                    break;
            }
        }

        private void NormalizeDeltaTime(float factor)
        {
            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = Time.timeScale * factor;
        
        }
        private void Prepare()
        {
            _listener = GetComponent<DebugSteeringEventsListener>();
        }
    }
}
