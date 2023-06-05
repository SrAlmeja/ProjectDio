//Raymundo cryoStorage Mosqueda 07/03/2023

using UnityEngine;
using CryoStorage;

namespace com.LazyGames.Dio
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Car_TimeControl))]
    
    public class CarImpulse : MonoBehaviour
    {
        [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;

        [Header("Serialized References")] 
        [SerializeField] private GameObject indicator;
        [SerializeField] private GameObject fighter;
        [SerializeField] private GameObject driverSeat;

        private float _impulseForce;
        private float _angleLerpSpeed;
        private float _indicatorOffset;
        private float _indicatorRadius;
        private float _fighterRadius;
        
        private Rigidbody _rb;
        private DebugSteeringEventsListener _listener;
        private VoidEventChannelSO _impulseEvent;
        private Vector3 _indicatorCenter;
        private Vector3 _indicatorOffsetVector;

        private float _currentFighterAngle;
        private float _targetFighterAngle;
        private float _currentIndicatorAngle;
        private float _targetIndicatorAngle;
        
        private Car_TimeControl _timeControl;

        private void Start()
        {
            Prepare();
        }

        private void Update()
        {
            Visualize();
            AngleSmoothing();
        }

        void Visualize()
        {
            ManageFighter();
            ManageIndicator();
        }

        private void ManageFighter()
        {
            if (!_timeControl.isSlow)
            {
                fighter.transform.position = driverSeat.transform.position;
                fighter.transform.rotation = driverSeat.transform.rotation;
            }
            else
            {
                _targetFighterAngle = CryoMath.AngleFromOffset(_listener.vec2Input);
                Vector3 pos = fighter.transform.position;
                pos = CryoMath.PointOnRadiusRelative(transform, _fighterRadius, _currentFighterAngle);
                fighter.transform.position = pos;
                fighter.transform.rotation = CryoMath.AimAtDirection(fighter.transform.position, transform.position);
            }
        }

        private void ManageIndicator()
        {
            indicator.SetActive(_timeControl.isSlow);
            if(!_timeControl.isSlow) return;
            _indicatorCenter = transform.position + _indicatorOffsetVector;
            Vector2 dir = new Vector2(_rb.velocity.x, _rb.velocity.z).normalized;
            _targetIndicatorAngle = CryoMath.AngleFromOffset(dir);
            indicator.transform.position = CryoMath.PointOnRadius(_indicatorCenter, _indicatorRadius, _currentIndicatorAngle);
            indicator.transform.rotation = CryoMath.AimAtDirection(_indicatorCenter, indicator.transform.position);
        }

        private void ApplyImpulse()
        {
            if (!_timeControl.isSlow) return;
            Vector3 dir = (transform.position - fighter.transform.position).normalized;
            _rb.AddForce(dir * _impulseForce, ForceMode.VelocityChange);
        }

        void AngleSmoothing()
        {
            if(!_timeControl.isSlow) return;
            LerpAngle(ref _currentFighterAngle, _targetFighterAngle, _angleLerpSpeed);
            LerpAngle(ref _currentIndicatorAngle, _targetIndicatorAngle, _angleLerpSpeed);
        }

        private void LerpAngle(ref float currentAngle, float targetAngle, float angleChangeSpeed)
        {
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, angleChangeSpeed * Time.fixedUnscaledDeltaTime);
        }
        
        private void Prepare()
        {
            // Load configurable values from Scriptable Object
            _impulseForce = carParametersSo.ImpulseForce;
            _angleLerpSpeed = carParametersSo.AngleLerpSpeed;
            _indicatorOffset = carParametersSo.IndicatorOffset;
            _indicatorRadius = carParametersSo.IndicatorRadius;
            _fighterRadius = carParametersSo.FighterRadius;
            
            _rb = GetComponent<Rigidbody>();
            _timeControl = GetComponent<Car_TimeControl>();
            _listener = GetComponent<DebugSteeringEventsListener>();
            
            //subscribes to impulse event
            _listener.DoImpulseEvent += ApplyImpulse;
            
            _indicatorOffsetVector = new Vector3(0, _indicatorOffset, 0);
        }
    }
}
