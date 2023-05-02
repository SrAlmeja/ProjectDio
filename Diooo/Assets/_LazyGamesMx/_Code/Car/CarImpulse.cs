//Raymundo cryoStorage Mosqueda 07/03/2023
//
using UnityEngine;
using CryoStorage;

namespace com.LazyGames.Dio
{
    public class CarImpulse : MonoBehaviour
    {
        [Header("Configurable Variables")]
        [SerializeField] private GameObject indicator;
        [SerializeField]private float impulseRadius = 4.5f;
        [SerializeField]private float impulseStrength = 5f;
        [SerializeField] private float impulseSens = .1f;
        [Tooltip("vertical offset of the center of the sphere")]
        [SerializeField] private float yOffset = .1f;

        private Vector3 offsetVector;
        private Rigidbody rb;
        private float impulseAngle;
        private Vector3 impulseCenter;
        private Vector3 impulsePos;
        private Vector3 impulseDir;
        private SteeringEventsListener _listener;

        // Start is called before the first frame update
        void Start()
        {
            Prepare();
        }

        // Update is called once per frame
        void Update()
        {
            GetDirection(); 
            DebugCheckInput();
            Visualize();
            impulseCenter = transform.position + new Vector3(0, yOffset, 0);
        }

        private void Visualize()
        {
            indicator.SetActive(_listener.stopTime);
            if(!_listener.stopTime)return;
            indicator.transform.position = impulsePos;
            indicator.transform.rotation = CryoMath.AimAtDirection(impulseCenter, impulsePos);
        }

        private void GetDirection()
        {
            // Vector2 vel = new Vector2(rb.velocity.x, rb.velocity.z);
            // var angle = CryoMath.AngleFromOffset(vel);
           impulsePos = CryoMath.PointOnRadius(impulseCenter, impulseRadius , impulseAngle);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(impulseCenter, impulseRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(impulsePos,.3f);
        }

        private void DebugCheckInput()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                if(! _listener.stopTime)return;
                impulseDir = impulsePos - transform.position + offsetVector;
                rb.AddForce(impulseDir.normalized * impulseStrength);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                impulseAngle += -impulseSens;
            }
            if (Input.GetKey(KeyCode.E))
            {
                impulseAngle += impulseSens;
            }
        }

        private void Prepare()
        {
            rb = GetComponent<Rigidbody>();
            impulseStrength = impulseStrength * rb.mass;
            _listener = GetComponent<SteeringEventsListener>();
            
        }
    }
}