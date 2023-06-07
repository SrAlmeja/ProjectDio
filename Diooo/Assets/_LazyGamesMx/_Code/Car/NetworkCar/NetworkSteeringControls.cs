using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;

public class NetworkSteeringControls : NetworkBehaviour
{
   
        [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;
        
        [Header("Serialized References")]
        [SerializeField] private WheelColliders wheelColliders;
        [SerializeField] private WheelMeshes wheelMeshes;

        [HideInInspector] public bool IsDrift;
        private AnimationCurve _enginePower;
        private float _brakeForce;
        private AnimationCurve _steerCurve;

        private float _speed;
        private float _wheelTurnSpeed;
        private float _currentAngle;
        private float _targetAngle;

        private float _currentRPM;
        
        private NetworkSteeringEventsListener _listener;
        private Rigidbody _rb;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            
            Prepare();
            _rb.centerOfMass = new Vector3(0, -0.1f, 0);

        }


        void Update()
        {
        
            UpdateWheels();
            ApplySteering();
            AirResist();
            ApplyHandBrake(_brakeForce);
            ApplyMotorTorque(_listener.rT);
            ApplyBrakeTorque(_listener.lT);
            _speed = _rb.velocity.magnitude;
        }

        private void ApplyMotorTorque(float torque)
        {
            wheelColliders.RLWheel.motorTorque = torque * _enginePower.Evaluate(_speed);
            wheelColliders.RRWheel.motorTorque = torque * _enginePower.Evaluate(_speed);
        }

        private void ApplyBrakeTorque(float torque)
        {
            wheelColliders.FLWheel.brakeTorque = torque * .3f;
            wheelColliders.FRWheel.brakeTorque = torque * .3f;
            wheelColliders.RLWheel.brakeTorque = torque * .7f;
            wheelColliders.RRWheel.brakeTorque = torque * .7f;
        }

        private void ApplyHandBrake(float torque)
        {
            if (!_listener.handBrake)
            {
                wheelColliders.FLWheel.brakeTorque = torque * _brakeForce;
                wheelColliders.FRWheel.brakeTorque = torque * _brakeForce;
                return;
            }
            wheelColliders.FLWheel.brakeTorque = 0;
            wheelColliders.FRWheel.brakeTorque = 0;
        }
        
        private void ApplySteering()
        {
            _targetAngle = _listener.angle;
            _currentAngle = LerpAngle();
            wheelColliders.FLWheel.steerAngle = _currentAngle * _steerCurve.Evaluate(_speed);
            wheelColliders.FRWheel.steerAngle = _currentAngle * _steerCurve.Evaluate(_speed);
        }

        
        private void AirResist()
        {
            _rb.AddForce(-transform.forward * (_speed * .3f));
            //downward force for stability
            _rb.AddForce(-transform.up * (_speed * .3f));
        }
        
        private void UpdateWheels()
        {
            UpdateWheel(wheelColliders.FLWheel, wheelMeshes.FLWheel);
            UpdateWheel(wheelColliders.FRWheel, wheelMeshes.FRWheel);
            UpdateWheel(wheelColliders.RLWheel, wheelMeshes.RLWheel);
            UpdateWheel(wheelColliders.RRWheel, wheelMeshes.RRWheel);
        }
        private void UpdateWheel(WheelCollider col, MeshRenderer rend)
        {
            col.GetWorldPose(out Vector3 pos, out Quaternion rot);
            rend.transform.position = pos;
            rend.transform.rotation = rot;
        }
        
        private float LerpAngle()
        {
            float result = Mathf.Lerp(_currentAngle, _targetAngle, _wheelTurnSpeed * Time.deltaTime);
            return result;
        }

        private void Prepare()
        {
            _steerCurve = carParametersSo.SteerCurve;
            _enginePower = carParametersSo.EnginePower;
            _wheelTurnSpeed = carParametersSo.WheelTurnSpeed;
            _brakeForce = carParametersSo.BrakeForce;
            
            _listener = GetComponent<NetworkSteeringEventsListener>();
            _rb = GetComponent<Rigidbody>();
        }
}
