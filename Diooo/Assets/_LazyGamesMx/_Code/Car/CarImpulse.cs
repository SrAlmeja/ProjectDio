//Raymundo cryoStorage Mosqueda 07/03/2023
//

using System.Collections;
using UnityEngine;
using CryoStorage;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class CarImpulse : MonoBehaviour
    {
        [Header("Configurable Variables")]
        [SerializeField]private float impulseRadius = 4.5f;
        [SerializeField]private float FighterRadius = 9f;
        [SerializeField]private float impulseStrength = 5f;
        [FormerlySerializedAs("yOffset")]
        [Tooltip("vertical offset of the center of the sphere")]
        [SerializeField] private float indicatorOffset = .1f;
        [FormerlySerializedAs("FighterOffset")] [SerializeField] private float fighterOffset = .5f;
        
        [Header("Serialized References")]
        [SerializeField] private GameObject indicator;
        [SerializeField] private GameObject fighter;
        [SerializeField] private GameObject DriverSeat;

        private Vector3 offsetVector;
        private Rigidbody rb;
        private float impulseAngle;
        private Vector3 impulseCenter;
        private Vector3 _fighterCenter;
        private Vector3 fighterPos;
        private Vector3 impulseDir;
        private DebugSteeringEventsListener _listener;
        private bool doStasis;

        // Start is called before the first frame update
        void Start()
        {
            Prepare();
        }

        // Update is called once per frame
        void Update()
        {
            Visualize();
            doStasis = _listener.stopTime;
        }

        private void Visualize()
        {
            indicator.SetActive(doStasis);
            if(!doStasis)return;
            MoveFighter();
            MoveIndicator();
            impulseCenter = transform.position + new Vector3(0, indicatorOffset, 0);
            _fighterCenter = transform.position + new Vector3(0, fighterOffset, 0);
        }

        private void MoveIndicator()
        {
            Vector2 dir = new Vector2(rb.velocity.x, rb.velocity.z).normalized;
            var dirAngle = CryoMath.AngleFromOffset(dir);
           indicator.transform.position = CryoMath.PointOnRadius(impulseCenter, impulseRadius , dirAngle);
           indicator.transform.rotation = CryoMath.AimAtDirection(impulseCenter, indicator.transform.position);

        }

        private void MoveFighter()
        {
            if (!doStasis)
            {
                fighter.transform.position = DriverSeat.transform.position;
            }
            else
            {
                fighter.transform.position = CryoMath.PointOnRadius(_fighterCenter, FighterRadius , _listener.angle);
                indicator.transform.rotation = CryoMath.AimAtDirection(_fighterCenter, fighter.transform.position);
            }
        }

        public void ApplyImpulse()
        {
            if(!_listener.stopTime)return;
            impulseDir = fighter.transform.position - transform.position;
            rb.AddForce(impulseDir.normalized * impulseStrength);
        }

        private void Prepare()
        {
            rb = GetComponent<Rigidbody>();
            impulseStrength = impulseStrength * rb.mass;
            _listener = GetComponent<DebugSteeringEventsListener>();
            
        }
    }
}
