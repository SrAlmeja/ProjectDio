using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryoStorage;
using Unity.Netcode;

namespace com.LazyGames.Dio
{
    public class NetworkCarImpulse : NetworkBehaviour
    {
        [Header("Configurable Variables")]
        [SerializeField] private GameObject indicator;
        [SerializeField]private float impulseRadius = 4.5f;
        [SerializeField]private float impulseStrength = 5f;
        // [SerializeField] private float impulseSens = .1f;
        [Tooltip("vertical offset of the center of the sphere")]
        [SerializeField] private float yOffset = .1f;

        private Vector3 offsetVector;
        private Rigidbody rb;
        private float impulseAngle;
        private Vector3 impulseCenter;
        private Vector3 impulsePos;
        private Vector3 impulseDir;
        private NetworkSteeringEventsListener _listener;

        // Start is called before the first frame update
        public override void OnNetworkSpawn()
        {
            if(!IsOwner) return;
            Prepare();
            base.OnNetworkSpawn();
        }

        // Update is called once per frame
        void Update()
        {
            if(!IsOwner) return;
            GetDirection(); 
            Visualize();
            impulseCenter = transform.position + new Vector3(0, yOffset, 0);
        }

        private void Visualize()
        {
            if(!IsOwner) return;
            if(!IsOwner) return;
            indicator.SetActive(_listener.stopTime);
            if(!_listener.stopTime)return;
            impulseAngle = _listener.rotate;
            indicator.transform.position = impulsePos;
            indicator.transform.rotation = CryoMath.AimAtDirection(impulseCenter, impulsePos);
        }

        private void GetDirection()
        {
            if(!IsOwner) return;
            //need to express current car velocity using indicator and its scale
            // Vector2 vel = new Vector2(rb.velocity.x, rb.velocity.z);
            // var angle = CryoMath.AngleFromOffset(vel);
           impulsePos = CryoMath.PointOnRadius(impulseCenter, impulseRadius , impulseAngle);
           
        }

        public void ApplyImpulse()
        {
            if(!IsOwner) return;
            if(!_listener.stopTime)return;
            impulseDir = impulsePos - transform.position;
            rb.AddForce(impulseDir.normalized * impulseStrength);
        }

        private void OnDrawGizmos()
        {
            if(!IsOwner) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(impulseCenter, impulseRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(impulsePos,.3f);
        }

        private void Prepare()
        {
            if(!IsOwner) return;
            rb = GetComponent<Rigidbody>();
            impulseStrength = impulseStrength * rb.mass;
            _listener = GetComponent<NetworkSteeringEventsListener>();
            
        }
    }
}
