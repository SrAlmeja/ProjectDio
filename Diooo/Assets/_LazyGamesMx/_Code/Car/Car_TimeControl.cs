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
        private float _stasisMeter = 1f;
        [SerializeField]private float _stasisRateOfChange = .3f;

        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            Debug.Log(_stasisMeter);
            Slow();
            Time.timeScale = currentTimeScale;
        }

        void Slow()
        {
            doSlow = _listener.stopTime;
            switch (doSlow)
            {
                case true:
                    currentTimeScale = targetTimeScale.Evaluate(_stasisMeter);
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = false;
                    _stasisMeter -= _stasisRateOfChange * Time.deltaTime;
                    if (_stasisMeter <= 0)
                    {
                        _stasisMeter = 0;
                        doSlow = false;
                    }
                    break;
                case false:
                    currentTimeScale = 1f;
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = true;
                    _stasisMeter += _stasisRateOfChange * Time.deltaTime;
                    if (_stasisMeter >= 1)
                    {
                        _stasisMeter = 1;
                    }
                    break;
            }
            // currentTimeScale = targetTimeScale;
            // currentTimeScale = .33f> Mathf.Abs(targetTimeScale-.1f) ? 1 : .01f;
            
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
