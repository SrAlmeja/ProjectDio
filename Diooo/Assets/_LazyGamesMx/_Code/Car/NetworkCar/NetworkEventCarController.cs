using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class NetworkEventCarController : NetworkBehaviour
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

        private NetworkSteeringEventsListener _steeringEventsListener;
        private float currentSteerAngle;
        private bool isBreaking;
        private float breakForce;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            
            Prepare();
            base.OnNetworkSpawn();
        }

        private void Update()
        {
            if (!IsOwner) return;
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        private void HandleMotor()
        {
            if (!IsOwner) return;
            frontLeftWheelCollider.motorTorque = _steeringEventsListener.torque  * motorForce;
            frontRightWheelCollider.motorTorque = _steeringEventsListener.torque * motorForce;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            if (!IsOwner) return;
            if (!IsOwner) return;
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
            if (!IsOwner) return;
            currentSteerAngle = maxSteerAngle * _steeringEventsListener.angle;
            frontLeftWheelCollider.steerAngle = currentSteerAngle;
            frontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels()
        {
            if (!IsOwner) return;
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            if (!IsOwner) return;
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        void Prepare()
        {
            if (!IsOwner) return;
            _steeringEventsListener = GetComponent<NetworkSteeringEventsListener>();
        }
    }
}
