using TMPro;
using UnityEngine;
using System.Collections;

namespace com.LazyGames.Dio
{
    public enum GearState
    {
        Neutral,
        Running,
        CheckingChange,
        Changing
    };

    public class DummyCarVelocityChange : MonoBehaviour
    {
        [SerializeField] private FloatEventChannelSO _angleEvent;
        [SerializeField] private FloatEventChannelSO _torqueEvent;

        private Rigidbody playerRB;
        public WheelColliders colliders;
        public WheelMeshes wheelMeshes;
        private float gasInput;
        private float brakeInput;
        private float steeringInput;
        public float motorPower;
        public float brakePower;
        public float slipAngle;
        public float speed;
        private float speedClamped;
        public AnimationCurve steeringCurve;

        public int isEngineRunning;

        public float RPM;
        public float redLine;
        public float idleRPM;
        //public TMP_Text rpmText;
        //public TMP_Text gearText;
        //public Transform rpmNeedle;
        public float minNeedleRotation;
        public float maxNeedleRotation;
        public int currentGear;

        public float[] gearRatios;
        public float differentialRatio;
        private float currentTorque;
        private float clutch;
        private float wheelRPM;
        public AnimationCurve hpToRPMCurve;
        private GearState gearState;
        public float increaseGearRPM;
        public float decreaseGearRPM;
        public float changeGearTime = 0.5f;

        public bool dummy = false;

        private void OnEnable()
        {
            _angleEvent.FloatEvent += Angle;
            _torqueEvent.FloatEvent += Torque;
        }

        private void OnDisable()
        {
            _angleEvent.FloatEvent -= Angle;
            _torqueEvent.FloatEvent -= Torque;
        }

        // Start is called before the first frame update
        void Start()
        {
            playerRB = gameObject.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isEngineRunning = 1;
            }
            //rpmNeedle.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, RPM / (redLine * 1.1f)));
            //rpmText.text = RPM.ToString("0,000") + "rpm";
            //gearText.text = (gearState == GearState.Neutral) ? "N" : (currentGear + 1).ToString();
            speed = colliders.RRWheel.rpm * colliders.RRWheel.radius * 2f * Mathf.PI / 10f;
            speedClamped = Mathf.Lerp(speedClamped, speed, Time.deltaTime);
            CheckInput();
            ApplyMotor();
            ApplySteering();
            ApplyBrake();
            ApplyWheelPositions();
        }

        void CheckInput()
        {
            if (Mathf.Abs(gasInput) > 0 && isEngineRunning == 0)
            {
                gearState = GearState.Running;
            }
            slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

            //fixed code to brake even after going on reverse by Andrew Alex 
            float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);
            if (gearState != GearState.Changing)
            {
                if (gearState == GearState.Neutral)
                {
                    clutch = 0;
                    if (Mathf.Abs(gasInput) > 0) gearState = GearState.Running;
                }
                else
                {
                    clutch = Input.GetKey(KeyCode.LeftShift) ? 0 : Mathf.Lerp(clutch, 1, Time.deltaTime);

                }
            }
            else
            {
                clutch = 0;
            }
            if (movingDirection < -0.5f && gasInput > 0)
            {
                brakeInput = Mathf.Abs(gasInput);
            }
            else if (movingDirection > 0.5f && gasInput < 0)
            {
                brakeInput = Mathf.Abs(gasInput);
            }
            else
            {
                brakeInput = 0;
            }
        }

        void Angle(float f)
        {
            steeringInput += f;
        }

        void Torque(float f)
        {
            gasInput += f;
        }

        void ApplyBrake()
        {
            colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
            colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;

            colliders.RRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
            colliders.RLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        }
        void ApplyMotor()
        {
            currentTorque = CalculateTorque();
            colliders.RRWheel.motorTorque = currentTorque * gasInput;
            colliders.RLWheel.motorTorque = currentTorque * gasInput;

        }

        float CalculateTorque()
        {
            float torque = 0;
            if (RPM < idleRPM + 200 && gasInput == 0 && currentGear == 0)
            {
                gearState = GearState.Neutral;
            }
            if (gearState == GearState.Running && clutch > 0)
            {
                if (RPM > increaseGearRPM)
                {
                    StartCoroutine(ChangeGear(1));
                }
                else if (RPM < decreaseGearRPM)
                {
                    StartCoroutine(ChangeGear(-1));
                }
            }
            if (isEngineRunning > 0)
            {
                if (clutch < 0.1f)
                {
                    RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLine * gasInput) + Random.Range(-50, 50), Time.deltaTime);
                }
                else
                {
                    wheelRPM = Mathf.Abs((colliders.RRWheel.rpm + colliders.RLWheel.rpm) / 2f) * gearRatios[currentGear] * differentialRatio;
                    RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM - 100, wheelRPM), Time.deltaTime * 3f);
                    torque = (hpToRPMCurve.Evaluate(RPM / redLine) * motorPower / RPM) * gearRatios[currentGear] * differentialRatio * 5252f * clutch;
                }
            }
            return torque;
        }

        void ApplySteering()
        {

            float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
            if (slipAngle < 120f)
            {
                steeringAngle += Vector3.SignedAngle(transform.forward, playerRB.velocity + transform.forward, Vector3.up);
            }
            steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
            colliders.FRWheel.steerAngle = steeringAngle;
            colliders.FLWheel.steerAngle = steeringAngle;
        }

        void ApplyWheelPositions()
        {
            UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
            UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
            UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
            UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
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
            var gas = Mathf.Clamp(Mathf.Abs(gasInput), 0.5f, 1f);
            return RPM * gas / redLine;
        }
        IEnumerator ChangeGear(int gearChange)
        {
            gearState = GearState.CheckingChange;
            if (currentGear + gearChange >= 0)
            {
                if (gearChange > 0)
                {
                    //increase the gear
                    yield return new WaitForSeconds(0.7f);
                    if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                if (gearChange < 0)
                {
                    //decrease the gear
                    yield return new WaitForSeconds(0.1f);

                    if (RPM > decreaseGearRPM || currentGear <= 0)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                gearState = GearState.Changing;
                yield return new WaitForSeconds(changeGearTime);
                currentGear += gearChange;
            }

            if (gearState != GearState.Neutral)
                gearState = GearState.Running;
        }
    }
    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FRWheel;
        public WheelCollider FLWheel;
        public WheelCollider RRWheel;
        public WheelCollider RLWheel;
    }
    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer FRWheel;
        public MeshRenderer FLWheel;
        public MeshRenderer RRWheel;
        public MeshRenderer RLWheel;
    }
}