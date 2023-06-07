using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;

public class NetworkCarAirControl : NetworkBehaviour
{
    [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;
        
        private float _torqueForce;
        private float _maxAngle;
        private float _yOffset;
        private float _raycastDistance;
        private Rigidbody _rb;
        private Vector3 _yOffSetVector;
        private bool grounded;
        
        private NetworkSteeringEventsListener listener;
        
        // Start is called before the first frame update

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            Prepare();
        }
        
        private void Update()
        {           
            if (!IsOwner) return;
            grounded = CheckGrounded();
        }
        private void FixedUpdate()
        {
            if (!IsOwner) return;

            if(grounded) return;
            AirControl();
        }

        private void AirControl()
        {
            Vector3 torque = new Vector3(GetDesiredPitch() * _torqueForce, 0f, -GetDesiredRoll() * _torqueForce);
            _rb.AddTorque(torque, ForceMode.Acceleration);

        }

        private float GetDesiredPitch()
        {
            float result = listener.vec2Input.y * _maxAngle;
            return result;
        }

        private float GetDesiredRoll()
        {
            float result = listener.vec2Input.x * _maxAngle;
            return result;
        }

        private bool CheckGrounded()
        {
            Transform t = transform;
            return Physics.Raycast(t.position + _yOffSetVector, -t.up.normalized , _raycastDistance);
        }

        private void OnDrawGizmos()
        {
            Transform t = transform;
            Vector3 tPos = t.position;
            Debug.DrawRay(tPos + _yOffSetVector, (-t.up).normalized * _raycastDistance, Color.magenta);
        }

        private void Prepare()
        {
            // Load configurable values from Scriptable Object
            _torqueForce = carParametersSo.TorqueForce;
            _maxAngle = carParametersSo.MaxAngle;
            _yOffset = carParametersSo.YOffset;
            _raycastDistance = carParametersSo.RaycastDistance;
            
            _rb = GetComponent<Rigidbody>();
            //caching offset vector
            _yOffSetVector = new Vector3(0, _yOffset, 0);

            listener = GetComponent<NetworkSteeringEventsListener>();

        }
}
