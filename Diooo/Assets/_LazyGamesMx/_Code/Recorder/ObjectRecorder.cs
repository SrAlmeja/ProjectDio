using UnityEngine;

namespace com.LazyGames.Dio
{
    public class ObjectRecorder : MonoBehaviour
    {
        [SerializeField] private float _recordInterval = 1.0f;
        [SerializeField] private RecordData _recordData;

        [SerializeField] private bool _isRecording;

        [Header("===Danger Zone===")]
        [SerializeField] private bool _eraseOnAwake;

        private float _lastRecordTime;

        private void Start()
        {
            if (_eraseOnAwake) _recordData.ClearData();
            _lastRecordTime = Time.time;
        }

        private void Update()
        {
            RecordDataPoint();
        }

        private void RecordDataPoint()
        {
            if (!_isRecording) return;

            if (Time.time - _lastRecordTime >= _recordInterval)
            {
                Vector3 position = transform.position;
                Quaternion rotation = transform.rotation;
                float time = Time.time;

                _recordData.AddRecord(position, rotation, time);

                _lastRecordTime = time;
            }
        }
    }
}

