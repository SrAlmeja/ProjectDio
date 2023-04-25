using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DummyCarVelocityChange : MonoBehaviour
    {
        [Header("Configurable Values")]
        [SerializeField] private float maxSteerAngle = 35f;
        [SerializeField] private float maxBreakForce = 8000f;
        [SerializeField] private float maxRPM= 8000f;
        [SerializeField] private float myRPM;

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
            float wheelRPM = _steeringEventsListener.torque;
            frontLeftWheelCollider.motorTorque = CalculateTorque(wheelRPM);
            frontRightWheelCollider.motorTorque = CalculateTorque(wheelRPM);
            ApplyBreaking();
        }

        private float CalculateTorque(float wheelRPM)
        {
            return wheelRPM >= myRPM ? 0 : (1 - wheelRPM / maxRPM) * _steeringEventsListener.torque * 5000;
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
            float rpm = CalculateRPM(wheelCollider);
            //myRPM = rpm;
        }

        private float CalculateRPM(WheelCollider wheelCollider)
        {
            float wheelRPM = wheelCollider.rpm;
            float wheelRadius = wheelCollider.radius;
            float rpm = wheelRPM * wheelRadius * 2 * Mathf.PI * 60 / 1000;
            return rpm;
        }

        void Prepare()
        {
            _steeringEventsListener = GetComponent<DebugSteeringEventsListener>();
            frontLeftWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
            frontRightWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
            rearLeftWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
            rearRightWheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
        }
    }
}