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
        [SerializeField] private bool doSlow;
        [SerializeField] private float targetTimeScale;
        
        private DebugSteeringEventsListener _listener;
        private float currentTimeScale = 1;
        private float savedMagnitude;
        private readonly float normalizeFactor = .02f;

        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            Slow();
            Time.timeScale = currentTimeScale;
        }

        void Slow()
        {
            doSlow = _listener.stopTime;
            switch (doSlow)
            {
                case true:
                    currentTimeScale = targetTimeScale;
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = false;
                    break;
                case false:
                    currentTimeScale = 1f;
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = true;
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
