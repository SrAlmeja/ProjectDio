
using TMPro;
using UnityEngine;
using System.Collections;

    public enum GearState
    {
        Neutral,
        Running,
        CheckingChange,
        Changing
    };

namespace com.LazyGames.Dio
{
    public class RaceCarController : MonoBehaviour
    {
        [SerializeField] private WheelColliders _colliders;
        [SerializeField] private WheelMeshes _wheelMeshes;
        [SerializeField] private float _motorPower = 100f;
        [SerializeField] private float _brakePower = 50000f;
        [SerializeField] private AnimationCurve _steeringCurve;
        [SerializeField] private float _redLine = 6500f;
        [SerializeField] private float _idleRPM = 800f;
        [SerializeField] private float[] _gearRatios = {3f,2.5f,2f,1.5f,1f,0.8f};
        [SerializeField] private float _differentialRatio = 4f;
        [SerializeField] private AnimationCurve _hpToRPMCurve;
        [SerializeField] private float _increaseGearRPM = 5500f;
        [SerializeField] private float _decreaseGearRPM = 300f;
        [SerializeField] private float _changeGearTime = 0.5f;

        private float _slipAngle;
        private float _speed;
        private float _RPM;
        private int _isEngineRunning;
        private int _currentGear;
        private Rigidbody _playerRB;
        private float _gasInput;
        private float _brakeInput;
        private float _steeringInput;
        private float _speedClamped;
        private float _currentTorque;
        private float _clutch;
        private float _wheelRPM;
        private GearState _gearState;

        private DebugSteeringEventsListener _listener;

        // Start is called before the first frame update
        void Start()
        {
            _playerRB = gameObject.GetComponent<Rigidbody>();
            _listener = gameObject.GetComponent<DebugSteeringEventsListener>();
        }

        void Update()
        {
            switch (_listener.handBrake)
            {
                case true:
                    _gearState = GearState.Neutral;
                    _isEngineRunning = 0;
                    _gasInput = 0;
                    _RPM = 0;
                    _speed--;
                    break;
                case false:
                    _speed = _colliders.RRWheel.rpm * _colliders.RRWheel.radius * 2f * Mathf.PI / 10f;
                    _speedClamped = Mathf.Lerp(_speedClamped, _speed, Time.deltaTime);
                    break;
            }
            CheckInput();
            ApplyMotor();
            ApplySteering();
            ApplyBrake();
            ApplyWheelPositions();
            KilometerPerHour();
            _gasInput = _listener.rT;
            _steeringInput = _listener.angle * _steeringCurve.Evaluate(_playerRB.velocity.magnitude);
        }

        void KilometerPerHour()
        {
            Vector3 velocity = _playerRB.velocity;
            float speedInKmPerHour = velocity.magnitude * 3.6f;
            int truncatedSpeed = Mathf.FloorToInt(speedInKmPerHour);
            // _kilometerPerHour.text = truncatedSpeed + "km/h";
        }

        void CheckInput()
        {
            if (Mathf.Abs(_gasInput) > 0 && _isEngineRunning == 0)
            {
                StartCoroutine(StartEngine());
                _gearState = GearState.Running;
            }
            _slipAngle = Vector3.Angle(transform.forward, _playerRB.velocity - transform.forward);

            //fixed code to brake even after going on reverse by Andrew Alex 
            float movingDirection = Vector3.Dot(transform.forward, _playerRB.velocity);
            if (_gearState != GearState.Changing)
            {
                if (_gearState == GearState.Neutral)
                {
                    _clutch = 0;
                    if (Mathf.Abs(_gasInput) > 0) _gearState = GearState.Running;
                }
                else
                {
                    _clutch = Input.GetKey(KeyCode.LeftShift) ? 0 : Mathf.Lerp(_clutch, 1, Time.deltaTime);
                }
            }
            else
            {
                _clutch = 0;
            }
            if (movingDirection < -0.5f && _gasInput > 0)
            {
                _brakeInput = Mathf.Abs(_gasInput);
            }
            else if (movingDirection > 0.5f && _gasInput < 0)
            {
                _brakeInput = Mathf.Abs(_gasInput);
            }
            else
            {
                _brakeInput = 0;
            }
        }

        void Angle(float f)
        {
            // if(_gasInput > 0) 
            // {
            //     _steeringInput = f * 150;
            // }
            // if(_gasInput == 0)
            // {
            //     _steeringInput = f * 50;
            // }
            // else if (_gasInput < 0)
            // {
            //     _steeringInput = -f * 50;
            // }
        }

        void Torque(float f)
        {
            _gasInput = f;
        }

        void ApplyBrake()
        {
            _colliders.FRWheel.brakeTorque = _brakeInput * _brakePower * 0.7f;
            _colliders.FLWheel.brakeTorque = _brakeInput * _brakePower * 0.7f;

            _colliders.RRWheel.brakeTorque = _brakeInput * _brakePower * 0.3f;
            _colliders.RLWheel.brakeTorque = _brakeInput * _brakePower * 0.3f;
        }
        void ApplyMotor()
        {
            _currentTorque = CalculateTorque();
            _colliders.RRWheel.motorTorque = _currentTorque * _gasInput;
            _colliders.RLWheel.motorTorque = _currentTorque * _gasInput;

        }

        float CalculateTorque()
        {
            float torque = 0;
            if (_RPM < _idleRPM + 200 && _gasInput == 0 && _currentGear == 0)
            {
                _gearState = GearState.Neutral;
            }
            if (_gearState == GearState.Running && _clutch > 0)
            {
                if (_RPM > _increaseGearRPM)
                {
                    StartCoroutine(ChangeGear(1));
                }
                else if (_RPM < _decreaseGearRPM)
                {
                    StartCoroutine(ChangeGear(-1));
                }
            }
            if (_isEngineRunning > 0)
            {
                if (_clutch < 0.1f)
                {
                    _RPM = Mathf.Lerp(_RPM, Mathf.Max(_idleRPM, _redLine * _gasInput) + Random.Range(-50, 50), Time.deltaTime);
                }
                else
                {
                    _wheelRPM = Mathf.Abs((_colliders.RRWheel.rpm + _colliders.RLWheel.rpm) / 2f) * _gearRatios[_currentGear] * _differentialRatio;
                    _RPM = Mathf.Lerp(_RPM, Mathf.Max(_idleRPM - 100, _wheelRPM), Time.deltaTime * 3f);
                    torque = (_hpToRPMCurve.Evaluate(_RPM / _redLine) * _motorPower / _RPM) * _gearRatios[_currentGear] * _differentialRatio * 5252f * _clutch;
                }
            }
            return torque;
        }

        void ApplySteering()
        {
            float steeringAngle = _steeringInput * _steeringCurve.Evaluate(_speed);
            if (_slipAngle < 120f)
            {
                steeringAngle += Vector3.SignedAngle(transform.forward, _playerRB.velocity + transform.forward, Vector3.up);
            }
            steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
            _colliders.FRWheel.steerAngle = steeringAngle;
            _colliders.FLWheel.steerAngle = steeringAngle;
        }

        void ApplyWheelPositions()
        {
            UpdateWheel(_colliders.FRWheel, _wheelMeshes.FRWheel);
            UpdateWheel(_colliders.FLWheel, _wheelMeshes.FLWheel);
            UpdateWheel(_colliders.RRWheel, _wheelMeshes.RRWheel);
            UpdateWheel(_colliders.RLWheel, _wheelMeshes.RLWheel);
        }
        
        void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
        {
            Quaternion quat;
            Vector3 position;
            coll.GetWorldPose(out position, out quat);
            wheelMesh.transform.position = position;
            wheelMesh.transform.rotation = quat;
        }
        public float GetSpeedRatio()
        {
            var gas = Mathf.Clamp(Mathf.Abs(_gasInput), 0.5f, 1f);
            return _RPM * gas / _redLine;
        }
        IEnumerator ChangeGear(int gearChange)
        {
            _gearState = GearState.CheckingChange;
            if (_currentGear + gearChange >= 0)
            {
                if (gearChange > 0)
                {
                    //increase the gear
                    yield return new WaitForSeconds(0.7f);
                    if (_RPM < _increaseGearRPM || _currentGear >= _gearRatios.Length - 1)
                    {
                        _gearState = GearState.Running;
                        yield break;
                    }
                }
                if (gearChange < 0)
                {
                    //decrease the gear
                    yield return new WaitForSeconds(0.1f);

                    if (_RPM > _decreaseGearRPM || _currentGear <= 0)
                    {
                        _gearState = GearState.Running;
                        yield break;
                    }
                }
                _gearState = GearState.Changing;
                yield return new WaitForSeconds(_changeGearTime);
                _currentGear += gearChange;
            }

            if (_gearState != GearState.Neutral)
                _gearState = GearState.Running;
        }

        IEnumerator StartEngine()
        {
            _isEngineRunning = 1;
            yield return new WaitForSeconds(0.6f);
            yield return new WaitForSeconds(0.4f);
            _isEngineRunning = 2;
        }
    }
}

    
