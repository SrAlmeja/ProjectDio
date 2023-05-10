using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class PlaybackData : MonoBehaviour
    {
        [Header("Settings")]
        public float PlaybackSpeed = 1f;
        public bool LoopPlayback = true;

        [SerializeField] private bool _playOnAwake;

        [Header("Dependencies")]
        [SerializeField] private RecordData _recordData;   

        private int _currentIndex = 0;  
        private float _elapsedTime = 0f; 
        private bool _isPlaying = false;

     
        void Start()
        {
            iTween.Init(gameObject);
            _currentIndex = 0;
            _elapsedTime = 0f;
           if(_playOnAwake) Play();
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

            _elapsedTime += Time.deltaTime * PlaybackSpeed;

            if (_elapsedTime >= _recordData.Records[_currentIndex].time)
            {
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

                ApplyRecording(_recordData, _currentIndex);
             
            }
        }

        private void ApplyRecording(RecordData _currentData, int index)
        {
            Hashtable hash = new Hashtable();
            hash.Add("position", _currentData.Records[index].position);
            hash.Add("rotation", _currentData.Records[index].rotation);
            hash.Add("time", _currentData.Records[index].time - _currentData.Records[index - 1].time);
            hash.Add("easetype", iTween.EaseType.easeInOutQuad);

            iTween.MoveTo(gameObject, hash);
        }
    }

}
