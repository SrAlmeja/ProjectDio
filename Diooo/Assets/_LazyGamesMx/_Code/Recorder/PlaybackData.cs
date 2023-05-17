using System.Collections;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class PlaybackData : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private RecordData _recordData;

        [Header("Settings")]
        public float PlaybackSpeed = 1f;
        public bool LoopPlayback = true;

        [Header("Playback Options")]
        [SerializeField] private bool _playOnAwake;
        [Tooltip("If checked, it will use iTween to interpolate values. Recommended for datasets of 1-3 recordings per second")]
        [SerializeField] private bool _useTweenInterpolation; 




        private int _currentIndex = 0;
        private float _elapsedTime = 0f;
        private bool _isPlaying = false;


        void Start()
        {
            iTween.Init(gameObject);
            _currentIndex = 0;
            _elapsedTime = 0f;
            if (_playOnAwake) Play();
        }

        // Update is called once per frame
        void Update()
        {
            ReadData();
        }

        public void Play()
        {
            _isPlaying = true;
            iTween.Resume(gameObject);
        }

        public void Stop()
        {
            _isPlaying = false;
            iTween.Pause(gameObject);
        }

        private void ReadData()
        {
            if (!_isPlaying) return;

            float duration = _recordData.Records[_recordData.Records.Count - 1].time - _recordData.StartTime;

            _elapsedTime += Time.deltaTime * PlaybackSpeed;

            if (_elapsedTime >= duration)
            {
                _elapsedTime = 0f;
                _currentIndex = 0;
            }

            float t = (_elapsedTime - _recordData.Records[_currentIndex].time + _recordData.StartTime) /
                      (_recordData.Records[_currentIndex + 1].time - _recordData.Records[_currentIndex].time);

            if (t > 1f)
            {
                t = 1f;
                _currentIndex++;
                if (_currentIndex >= _recordData.Records.Count)
                {
                    if (LoopPlayback)
                    {
                        _currentIndex = 0;
                    }
                    else
                    {
                        Stop();
                        return;
                    }
                }
            }

            if(_useTweenInterpolation)
            {
            ApplyRecording(_recordData, _currentIndex);
            } else
            {
            ApplyRecordingLerped(_recordData, _currentIndex, t);
            }
        }


        private void ApplyRecording(RecordData _currentData, int index)
        {
            Hashtable hash = new Hashtable();
            hash.Add("position", _currentData.Records[index].position);
            hash.Add("rotation", _currentData.Records[index].rotation.eulerAngles);
            hash.Add("time", _currentData.Records[index].time - (_currentIndex > 0 ? _currentData.Records[index - 1].time : 0f));
            hash.Add("easetype", iTween.EaseType.easeInOutQuad);

            iTween.MoveTo(gameObject, hash);
            iTween.RotateTo(gameObject, _currentData.Records[index].rotation.eulerAngles, hash["time"] as float? ?? 0f);
        }

        private void ApplyRecordingLerped(RecordData currentData, int index, float t)
        {
            var targetPosition = currentData.Records[index].position;
            var targetRotation = currentData.Records[index].rotation;

            transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
        }
    }
}
