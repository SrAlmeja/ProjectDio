using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class NetworkCarTimeControl : NetworkBehaviour
    {
        [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;
        
       private AnimationCurve _targetTimeScale;
       private float _fillDuration;
       private float _emptyDuration;

        private NetworkSteeringEventsListener _listener;
        private NetworkCar_Respawn _carRespawn;
        
        private float savedMagnitude;
        private float currentTimeScale = 1;
        private readonly float normalizeFactor = .02f;
        private float _stasisFillDelta;
        private float _stasisEmptyDelta;
        private float _stasisMeter;
        private float _fadeSpeed;
        private float targetOpacity;
        private float _currentOpacity;
        private bool _doSlow;
        private float _elapsedTime;
        
        [HideInInspector]public bool isSlow;

        [SerializeField] private GameObject fillIndicator;
        private Renderer _stasisIndicatorRenderer;

        private float StasisMeterClamped
        {
            get => _stasisMeter;
            set => _stasisMeter = Mathf.Clamp01(value);
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
                
            Prepare();
        }
        
        private void Update()
        {
            if (!IsOwner) return;
                
            ManageTimeScale();
            ManageMeter();
            
            Time.timeScale = currentTimeScale;
            _stasisIndicatorRenderer.material.SetFloat("_FillValue", StasisMeterClamped);
            _stasisIndicatorRenderer.material.SetFloat("_Opacity", _currentOpacity);
        }

        private void DoSlow()
        {
            if (!IsOwner) return;

            if (!CheckStasis())
            {
                _doSlow = false;
                return;
            }
            _doSlow = true;
        }

        void ManageTimeScale()
        {
            if (!IsOwner) return;

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
            if (!IsOwner) return;

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
            ManageOpacity();
        }

        private void ManageOpacity()
        {
            if (!IsOwner) return;

            if (StasisMeterClamped >= 1f)
            {
                targetOpacity = 0;
            } else{targetOpacity = 1;}
            _currentOpacity = Mathf.Lerp(_currentOpacity,targetOpacity,_fadeSpeed * Time.fixedUnscaledDeltaTime);
        }

        private void NormalizeDeltaTime(float factor)
        {
            if (!IsOwner) return;

            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = Time.timeScale * factor;
        }

        private float GetIncrement(float duration)
        {
           float result = (1 - 0 + Mathf.Epsilon) / duration;
           return result;
        }

        private void OnOnDie()
        {
            if (!IsOwner) return;
            _doSlow = false;
        }

        private void Prepare()
        {
            // Load configurable values from Scriptable Object
            _targetTimeScale = carParametersSo.TargetTimeScale;
            _fillDuration = carParametersSo.FillDuration;
            _emptyDuration = carParametersSo.EmptyDuration;
            _fadeSpeed = carParametersSo.FadeSpeed;
            
            _listener = GetComponent<NetworkSteeringEventsListener>();
            
            //subscribes to stopTime event
            _listener.DoStopTimeEvent += DoSlow;
            
            _stasisFillDelta = GetIncrement((_fillDuration + 3)*2);
            _stasisEmptyDelta = GetIncrement((_emptyDuration + 3) * 2);

            _stasisIndicatorRenderer = fillIndicator.GetComponent<MeshRenderer>();
            _carRespawn = GetComponent<NetworkCar_Respawn>();
            
            _carRespawn.OnDie += OnOnDie;
        }
    }
}
