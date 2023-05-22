using UnityEngine;

namespace com.LazyGames.Dio
{
    public class ObjectRecorder : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private RecordData _recordData;

        [Header("Record at")]
        [Tooltip("Records at this given time if 'atFramerate' is disabled")]
        [SerializeField] private float _recordInterval = 1.0f;
        [Tooltip("Records at the game update speed when enabled. Ignores recordInterval")]
        [SerializeField] private bool _atFramerate;

        [Header("Start recording right away")]
        [SerializeField] private bool _isRecording;

        [Header("===Danger Zone===")]
        [SerializeField] private bool _eraseOnAwake;

        private float _lastRecordTime;

        private void Start()
        {
            if (_eraseOnAwake) _recordData.ClearData();
            if (_recordData.Records.Count == 0) _recordData.StartTime = Time.time;
            _lastRecordTime = Time.time;
        }

        private void Update()
        {
            RecordDataPoint();
        }

        private void RecordDataPoint()
        {
            if (!_isRecording) return;

            if (_atFramerate)
            {
                AddData();
            } else if (Time.time - _lastRecordTime >= _recordInterval)
            {
               AddData();
            }
        }

        private void AddData()
        {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            float time = Time.time;

            _recordData.AddRecord(position, rotation, time);

            _lastRecordTime = time;
        }
    }
}

