// Creado Raymundo "CryoStorage" Mosqueda 05/06/2023

using System;
using QFSW.QC;
using Unity.Mathematics;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class SteeringControls : MonoBehaviour
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
        
        private DebugSteeringEventsListener _listener;
        private Rigidbody _rb;

        void Start()
        {
            Prepare();
            _rb.centerOfMass = new Vector3(0, -0.1f, 0);
        }

        void Update()
        {
        
            UpdateWheels();
            ApplySteering();
            AirResist();
            GetTorques();
            // ApplyHandBrake(_brakeForce);
            _speed = _rb.velocity.magnitude;
        }
        
        private void GetTorques()
        {
            float slip = GetSlip();

            if (_listener.rT > 0)
            {
                if (slip <= 60)
                {
                    // Moving forward, sideways, or not at all
                    ApplyMotorTorque(_listener.rT * _enginePower.Evaluate(_speed));
                }
                else if (slip > 150)
                {
                    // Moving backward
                    ApplyMotorTorque(0);
                    ApplyBrakeTorque(_listener.rT * _brakeForce);
                }
            }
            else if (_listener.lT > 0)
            {
                if (slip <= 150)
                {
                    // Moving forward or sideways
                    ApplyMotorTorque(0);
                    ApplyBrakeTorque(_listener.lT * _brakeForce);
                }
                else if (slip <= 180)
                {
                    // Stationary or very slow backward movement
                    ApplyMotorTorque(-_listener.lT * _enginePower.Evaluate(_speed));
                    ApplyBrakeTorque(0);
                }
            }
            else
            {
                // No input
                if (slip <= 180)
                {
                    // Stationary or very slow backward movement
                    ApplyMotorTorque(0);
                    ApplyBrakeTorque(0);
                }
                else
                {
                    // Moving backward
                    ApplyMotorTorque(-_listener.lT * _enginePower.Evaluate(_speed));
                    ApplyBrakeTorque(0);
                }
            }
        }
        
        private float GetSlip()
        {
            float result = Vector3.Angle(_rb.velocity.normalized, transform.forward);
            return result;
        }

        private void ApplyMotorTorque(float torque)
        {
            wheelColliders.RLWheel.motorTorque = torque;
            wheelColliders.RRWheel.motorTorque = torque;
        }

        private void ApplyBrakeTorque(float torque)
        {
            wheelColliders.FLWheel.brakeTorque = torque * .3f;
            wheelColliders.FRWheel.brakeTorque = torque * .3f;
            wheelColliders.RLWheel.brakeTorque = torque * .7f;
            wheelColliders.RRWheel.brakeTorque = torque * .7f;
        }

        // private void ApplyHandBrake(float torque)
        // {
        //     if (!_listener.handBrake)
        //     {
        //         wheelColliders.FLWheel.brakeTorque = torque * _brakeForce;
        //         wheelColliders.FRWheel.brakeTorque = torque * _brakeForce;
        //         return;
        //     }
        //     wheelColliders.FLWheel.brakeTorque = 0;
        //     wheelColliders.FRWheel.brakeTorque = 0;
        // }
        
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


            _listener = GetComponent<DebugSteeringEventsListener>();
            _rb = GetComponent<Rigidbody>();
        }
    }
}
