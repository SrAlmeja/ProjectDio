// Creado Raymundo Mosqueda 24/04/2023
//

using System;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DebugEventCarController : MonoBehaviour
    {
        
        [Header("Configurable Values")]
        [SerializeField] private float motorForce = 2000f;
        [SerializeField] private float maxSteerAngle = 35f;
        [SerializeField] private float maxBreakForce = 8000f;
        
        [Header("Serialized Wheel Colliders")]
        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;
        [SerializeField] private WheelCollider rearLeftWheelCollider;
        [SerializeField] private WheelCollider rearRightWheelCollider;

        [Header("Serialized Wheel Transforms")]
        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheeTransform;
        [SerializeField] private Transform rearLeftWheelTransform;
        [SerializeField] private Transform rearRightWheelTransform;

        private DebugSteeringEventsListener _steeringEventsListener;
        private float currentSteerAngle;
        private bool isBreaking;
        private float breakForce;
        
        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        private void HandleMotor()
        {
            frontLeftWheelCollider.motorTorque = _steeringEventsListener.torque  * motorForce;
            frontRightWheelCollider.motorTorque = _steeringEventsListener.torque * motorForce;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            frontRightWheelCollider.brakeTorque = breakForce;
            frontLeftWheelCollider.brakeTorque = breakForce;
            switch (_steeringEventsListener.handBrake)
            {
                case true:
                    breakForce = maxBreakForce;
                    break;
                case false:
                    breakForce = 0;
                    break;
            }
        }

        private void HandleSteering()
        {
            if (_steeringEventsListener.stopTime) return;
            currentSteerAngle = maxSteerAngle * _steeringEventsListener.angle;
            frontLeftWheelCollider.steerAngle = currentSteerAngle;
            frontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        void Prepare()
        {
            _steeringEventsListener = GetComponent<DebugSteeringEventsListener>();
        }
    }
}