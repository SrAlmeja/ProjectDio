//creado raymundo mosqueda 29/03/2023
//

using System;

namespace com.LazyGames.Dio 
{
    public class EventCarController : GDC_CarController
    {
        private float maxBreakForce;
        private SteeringEventsListener _listener;

        public override void OnNetworkSpawn()
        {
            Prepare();
        }

        protected override void FixedUpdate()
        { /* truncated from base class */ }

        private void Update()
        {
            HandleMotor();
            HandleSteering();
            
        }

        protected override void HandleMotor()
        {
            frontLeftWheelCollider.motorTorque = _listener.torque * motorForce;
            frontRightWheelCollider.motorTorque = _listener.torque * motorForce;
            ApplyBreaking();    
        }
        
        protected override void HandleSteering()
        {
            frontLeftWheelCollider.steerAngle = _listener.angle * maxSteerAngle;
            frontRightWheelCollider.steerAngle = _listener.angle * maxSteerAngle;
        }

        protected override void ApplyBreaking()
        {
            frontRightWheelCollider.brakeTorque = breakForce;
            frontLeftWheelCollider.brakeTorque = breakForce;
            switch (_listener.handBrake)
            {
                case true:
                    breakForce = maxBreakForce;
                    break;
                case false:
                    breakForce = 0;
                    break;
            }
        }
        
        


        private void Prepare()
        {
            maxBreakForce = breakForce;
            _listener = GetComponent<SteeringEventsListener>();
        }
    }   
}
