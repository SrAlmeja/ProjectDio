//Raymundo cryoStorage Mosqueda 07/03/2023
//

using System;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace com.LazyGames.Dio
{
    public class Car_TimeControl : MonoBehaviour
    {
        [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;
        
       private AnimationCurve _targetTimeScale;
       private float _fillDuration;
       private float _emptyDuration;

        private DebugSteeringEventsListener _listener;
        
        private float savedMagnitude;
        private float currentTimeScale = 1;
        private readonly float normalizeFactor = .02f;
        private float _stasisFillDelta;
        private float _stasisEmptyDelta;
        private float _stasisMeter;
        private bool _doSlow;
        
        [HideInInspector]public bool isSlow;

        [SerializeField] private GameObject fillIndicator;
        private Renderer _fillIndicatorRenderer;

        private float StasisMeterClamped
        {
            get => _stasisMeter;
            set => _stasisMeter = Mathf.Clamp01(value);
        }
        
        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            ManageTimeScale();
            ManageMeter();
            Time.timeScale = currentTimeScale;
            _fillIndicatorRenderer.material.SetFloat("_FillValue", StasisMeterClamped);
        }

        private void DoSlow()
        {
            if (!CheckStasis())
            {
                _doSlow = false;
                return;
            }
            _doSlow = true;
        }

        void ManageTimeScale()
        {
            switch (_doSlow)
            {
                case true:
                    isSlow = true;
                    currentTimeScale = _targetTimeScale.Evaluate(StasisMeterClamped);
                    NormalizeDeltaTime(normalizeFactor);
                    break;
                case false:
                    isSlow = false;
                    currentTimeScale = 1f;
                    NormalizeDeltaTime(normalizeFactor);
                    break;
            }
        }
        
        bool CheckStasis()
        {
            if (StasisMeterClamped >= .99f)
            {
                return true;
            }
            return false;
        }

        private void ManageMeter()
        {
            switch (isSlow)
            {
                case true:
                    StasisMeterClamped -= _stasisEmptyDelta * Time.fixedUnscaledDeltaTime;
                    break;
                case false:
                    StasisMeterClamped += _stasisFillDelta * Time.fixedUnscaledDeltaTime;
                    break;
            }
            if(StasisMeterClamped <= 0)
            {
                _doSlow = false;
            }
        }

        private void NormalizeDeltaTime(float factor)
        {
            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = Time.timeScale * factor;
        }

        private float GetIncrement(float duration)
        {
           float result = (1 - 0 + Mathf.Epsilon) / duration;
           return result;
        }

        private void Prepare()
        {
            // Load configurable values from Scriptable Object
            _targetTimeScale = carParametersSo.TargetTimeScale;
            _fillDuration = carParametersSo.FillDuration;
            _emptyDuration = carParametersSo.EmptyDuration;
            
            _listener = GetComponent<DebugSteeringEventsListener>();
            
            //subscribes to stopTime event
            _listener.DoStopTimeEvent += DoSlow;
            _stasisFillDelta = GetIncrement((_fillDuration + 3)*2);
            _stasisEmptyDelta = GetIncrement((_emptyDuration + 3) * 2);

            _fillIndicatorRenderer = fillIndicator.GetComponent<MeshRenderer>();
        }
    }
}
