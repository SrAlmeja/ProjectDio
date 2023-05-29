//Raymundo cryoStorage Mosqueda 07/03/2023

using UnityEngine;
using CryoStorage;

namespace com.LazyGames.Dio
{
    public class CarImpulse : MonoBehaviour
    {
        [Header("Configurable Variables")] [SerializeField]
        private float impulseForce = 10f;
        [SerializeField] private float angleLerpSpeed = 1f;

        [Header("Indicator Variables")] [SerializeField]
        private float indicatorOffset = .3f;

        [SerializeField] private float indicatorRadius = 3f;

        [Header("Indicator Variables")] [SerializeField]
        private float fighterOffset = .5f;

        [SerializeField] private float fighterRadius = 3f;

        [Header("Serialized References")] [SerializeField]
        private GameObject indicator;

        [SerializeField] private GameObject fighter;
        [SerializeField] private GameObject driverSeat;

        private Rigidbody _rb;
        private DebugSteeringEventsListener _listener;
        private VoidEventChannelSO _impulseEvent;
        private Vector3 _indicatorCenter;
        private Vector3 _fighterCenter;
        private Vector3 _indicatorOffsetVector;
        private Vector3 _fighterOffsetVector;
        private bool doStasis;

        private float _currentFighterAngle;
        private float _targetFighterAngle;
        private float _currentIndicatorAngle;
        private float _targetIndicatorAngle;

        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            Visualize();
            AngleSmoothing();
            doStasis = _listener.stopTime;
            

        }

        void Visualize()
        {
            ManageFighter();
            ManageIndicator();
        }

        private void ManageFighter()
        {
            if (!doStasis)
            {
                _fighterCenter = transform.position + _fighterOffsetVector;
                fighter.transform.position = driverSeat.transform.position;
                fighter.transform.rotation = driverSeat.transform.rotation;
            }
            else
            {
                _fighterCenter = transform.position + _fighterOffsetVector;
                _targetFighterAngle = CryoMath.AngleFromOffset(_listener.Vec2Input);
                Vector3 pos = fighter.transform.position;
                pos = CryoMath.PointOnRadiusRelative(transform, fighterRadius, _currentFighterAngle);
                fighter.transform.position = pos;
                fighter.transform.rotation = CryoMath.AimAtDirection(fighter.transform.position, transform.position);
            }
        }

        private void ManageIndicator()
        {
            indicator.SetActive(doStasis);
            if(!doStasis) return;
            _indicatorCenter = transform.position + _indicatorOffsetVector;
            Vector2 dir = new Vector2(_rb.velocity.x, _rb.velocity.z).normalized;
            _targetIndicatorAngle = CryoMath.AngleFromOffset(dir);
            indicator.transform.position = CryoMath.PointOnRadius(_indicatorCenter, indicatorRadius, _currentIndicatorAngle);
            indicator.transform.rotation = CryoMath.AimAtDirection(_indicatorCenter, indicator.transform.position);
        }

        private void ApplyImpulse()
        {
            Vector3 dir = (transform.position - fighter.transform.position).normalized;
            _rb.AddForce(dir * impulseForce, ForceMode.VelocityChange);
        }

        void AngleSmoothing()
        {
            LerpAngle(ref _currentFighterAngle, _targetFighterAngle, angleLerpSpeed);
            LerpAngle(ref _currentIndicatorAngle, _targetIndicatorAngle, angleLerpSpeed);
        }

        private void LerpAngle(ref float currentAngle, float targetAngle, float angleChangeSpeed)
        {
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, angleChangeSpeed * Time.deltaTime);
        }
        
        private void Prepare()
        {
            _rb = GetComponent<Rigidbody>();
            _listener = GetComponent<DebugSteeringEventsListener>();
            
            //subscribes to impulse event
            _listener._doImpulseEvent += ApplyImpulse;
            
            _fighterOffsetVector = new Vector3(0, fighterOffset, 0);
            _indicatorOffsetVector = new Vector3(0, indicatorOffset, 0);
        }
    }
}
