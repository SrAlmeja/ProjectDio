// Creado Raymundo Mosqueda 24/04/2023
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Car_LocalTimeControl : MonoBehaviour
    {
        [SerializeField] private float slowFactor = .03f;
        
        private DebugSteeringEventsListener _steeringEventsListener;
        private Rigidbody _rigidbody;

        private float maxSlowFactor;
        // Start is called before the first frame update
        void Start()
        {
            Prepare();
        }

        // Update is called once per frame
        void Update()
        {
            StopTime();
        }

        private void StopTime()
        {
            _rigidbody.velocity = _rigidbody.velocity * slowFactor;
            _rigidbody.angularDrag = _rigidbody.angularDrag * slowFactor;
            Debug.Log($"StopTime: {_steeringEventsListener.stopTime}");

            switch (_steeringEventsListener.stopTime)
            {
                case true:
                    slowFactor = maxSlowFactor;
                    break;
                case false:
                    slowFactor = 1;
                    break;
            }
        }
        
        private void Prepare()
        {
            _steeringEventsListener = GetComponent<DebugSteeringEventsListener>();
            _rigidbody = GetComponent<Rigidbody>();
            maxSlowFactor = slowFactor;
        }
    }
}
