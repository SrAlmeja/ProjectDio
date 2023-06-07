//Fernando Cossio 02/05/2023

using System;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class StopWatchManager : MonoBehaviour
    {
        [Header("So Channel Dependencies")]
        [SerializeField] private BoolEventChannelSO _racePaused;
        [SerializeField] private VoidEventChannelSO _timeRequest;
        [SerializeField] private GenericDataEventChannelSO _timeResponse;

        private float _stopWatch = 0;
        private bool _paused = true;

        public float CurrentTime { get { return _stopWatch; } }

        public static StopWatchManager Instance;

        private void Awake()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            _racePaused.BoolEvent += UpdatePause;
            _timeRequest.VoidEvent += SendTime;
        }

        private void OnDisable()
        {
            _racePaused.BoolEvent -= UpdatePause;
            _timeRequest.VoidEvent -= SendTime;
        }
        
        private void Update()
        {
            AddTime();
        }

        private void AddTime()
        {
            if (_paused) return;

            _stopWatch += Time.deltaTime;
        }

        private void UpdatePause(bool status)
        {
            _paused = status;
        }

        private void SendTime()
        {
            _timeResponse.DoubleEvent.Invoke(_stopWatch);
        }
        public void ResetTime()
        {
            _stopWatch = 0;
        }
        
        
        
    }
}

