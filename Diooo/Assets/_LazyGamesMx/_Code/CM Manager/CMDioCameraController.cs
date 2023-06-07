using System;
using UnityEngine;
using Cinemachine;
using com.LazyGames.Dio;

public class CMDioCameraController : MonoBehaviour
{
    [Header("Dependenices")]
    [SerializeField] private CinemachineFreeLook _cmVirtualCamera;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _playerRigidBody;
    [SerializeField] private ParticleSystem _speedLinesPS;

    [Header("Settings")]
    [Header("Maximum theoretical speed")]
    [Tooltip("Everything is calculated interpolated between speed 0 and this maximum speed.")]
    [SerializeField] private float _maxSpeed = 1f;

    [Header("Camera Locks")]
    [SerializeField] private bool _lockCamera = true;
    [Range(0f,1f)]
    [SerializeField] private float _speedToStartLockCamera = 0.2f;
    [Range(0f, 1f)]
    [Tooltip("At this speed % the camera reaches maximum restrictions, be sure to set it higher than SpeedToLockCamera")]
    [SerializeField] private float _speedToTotalLock = 0.8f;

    [Header("Particles")]
    [Range (0,1)]
    [SerializeField] private float _speedLinesStartPercentage = 0.2f;

    [Header("Debug")]
    [SerializeField] private bool _debugValues = false;

    private CinemachineBasicMultiChannelPerlin[] _cmRigPerlinNoise = new CinemachineBasicMultiChannelPerlin[3];

    private ParticleSystem.MainModule _mainPS;
    private ParticleSystem.EmissionModule _emissionPS;

    private float _currentSpeed = 0;
    private float _currentFOV;
    private float _currentAmp;
    private float _currentFreq;

    //CineMachine Camera settings
    private const float _minFOV = 40f;
    private const float _maxFOV = 60f;

    private const float _minAmp = 0.1f; //This settings control the camera shake as the car goes faster
    private const float _maxAmp = 1.0f;

    private const float _minFreq = 0.1f;
    private const float _maxFreq = 5.0f;

    private const float _maxParticlesSpeed = 15;
    private const float _maxParticlesRate = 100;

    private void OnEnable()
    {
        CheckDependencies();
        GetRigs();
    }
    

    private void PrepareNetworkCamera()
    {
        Debug.Log("PrepareNetworkCamera");  
        CheckDependencies();
    }
    
    public void SetTargetNetwork(Transform target)
    {
        Debug.Log("SetTargetNetwork");
        _playerTransform = target;
        _playerRigidBody = target.GetComponent<Rigidbody>();
        PrepareNetworkCamera();
        
    }
    private void Update()
    {
        if (_playerRigidBody == null || _playerTransform == null)
            return;
        
        _currentSpeed = SpeedPercentage(); //Always leave this on top. It calculates % of the max speed. 
        ChangeFOV();
        ChangeShake();
        ChangeParticles();
        CheckCameraLock();
        DebugValues();
    }
    
    private void ChangeFOV()
    {
        _currentFOV = LerpValue(_minFOV, _maxFOV, _currentSpeed);
        _cmVirtualCamera.m_Lens.FieldOfView = _currentFOV;
    }

    private void ChangeShake()
    {
        _currentAmp = LerpValue(_minAmp, _maxAmp, _currentSpeed);
        _currentFreq = LerpValue(_minFreq, _maxFreq, _currentSpeed);

        for (int i = 0; i < 3; i++)
        {
            _cmRigPerlinNoise[i].m_AmplitudeGain = _currentAmp;
            _cmRigPerlinNoise[i].m_FrequencyGain = _currentFreq;
        }
    }

    private void ChangeParticles()
    {
        if (_currentSpeed >= _speedLinesStartPercentage)
        {
            float startLerpValue = Mathf.Lerp(0, 1, _speedLinesStartPercentage);
            float currentLerpValue = Mathf.Lerp(startLerpValue, 1, (_currentSpeed - _speedLinesStartPercentage) / (1 - _speedLinesStartPercentage));

            _mainPS.startSpeed = currentLerpValue * _maxParticlesSpeed;
            _emissionPS.rateOverTime = currentLerpValue * _maxParticlesRate;
        }
        else
        {
            _mainPS.startSpeed = 0f;
            _emissionPS.rateOverTime = 0f;
        }
    }

    private float SpeedPercentage()
    {
        float speedPercentage = Mathf.Clamp01(_playerRigidBody.velocity.magnitude / _maxSpeed);
        return speedPercentage;
    }

    private void CheckCameraLock()
    {
        if (!_lockCamera || _currentSpeed < _speedToStartLockCamera)
        {
            UnlockCamera();
            return;
        } else if(_currentSpeed >= _speedToStartLockCamera)    
        {
          LockCamera();
        }
    }

    private void LockCamera()
    {
        float cameraLockInterpolation = Mathf.InverseLerp(_speedToStartLockCamera, _speedToTotalLock, _currentSpeed);
        _cmVirtualCamera.m_RecenterToTargetHeading.m_enabled = true;
        _cmVirtualCamera.m_RecenterToTargetHeading.m_WaitTime = LerpValue(15f,0.1f, cameraLockInterpolation);
        _cmVirtualCamera.m_RecenterToTargetHeading.m_RecenteringTime = LerpValue(3f, 0.1f, cameraLockInterpolation);
        _cmVirtualCamera.m_XAxis.m_MinValue = LerpValue(-180f, -0.1f, cameraLockInterpolation);
        _cmVirtualCamera.m_XAxis.m_MaxValue = LerpValue(180f, 0.1f, cameraLockInterpolation);
        _cmVirtualCamera.m_XAxis.m_Wrap = false;
        _cmVirtualCamera.m_YAxisRecentering.m_enabled = true;
        _cmVirtualCamera.m_YAxisRecentering.m_WaitTime = LerpValue(15f, 0.1f, cameraLockInterpolation);
        _cmVirtualCamera.m_YAxisRecentering.m_RecenteringTime = LerpValue(3f, 0.1f, cameraLockInterpolation);
    }

    private void UnlockCamera()
    {
        _cmVirtualCamera.m_RecenterToTargetHeading.m_enabled = false;
        _cmVirtualCamera.m_RecenterToTargetHeading.m_WaitTime = 15f;
        _cmVirtualCamera.m_RecenterToTargetHeading.m_RecenteringTime = 3f;
        _cmVirtualCamera.m_XAxis.m_MinValue = -180f;
        _cmVirtualCamera.m_XAxis.m_MaxValue = 180f;
        _cmVirtualCamera.m_XAxis.m_Wrap = true;
        _cmVirtualCamera.m_YAxisRecentering.m_enabled = false;
        _cmVirtualCamera.m_YAxisRecentering.m_WaitTime = 15f;
        _cmVirtualCamera.m_YAxisRecentering.m_RecenteringTime = 3f;
    }

    private float LerpValue(float min, float max, float lerpTo)
    {
        float value = Mathf.Lerp(min, max, lerpTo);
        return value;
    }

    private void CheckDependencies()
    {
        if (_cmVirtualCamera == null) Debug.LogError("No CineMachine camera set in dependencies for CM Camera Controller");
        if (_playerTransform == null) Debug.LogError("No player transform set in dependencies for CM Camera Controller");
        if (_playerRigidBody == null) Debug.LogError("No player rigidbody set in dependencies for CM Camera Controller");
        if (_cmVirtualCamera.LookAt == null) _cmVirtualCamera.LookAt = _playerTransform;
        if (_cmVirtualCamera.Follow == null) _cmVirtualCamera.Follow = _playerTransform;

        _mainPS = _speedLinesPS.main;
        _emissionPS = _speedLinesPS.emission;
    }

    private void GetRigs()
    {
       for(int i = 0; i < 3; i++)
        {
            _cmRigPerlinNoise[i] = _cmVirtualCamera.GetRig(i).GetCinemachineComponent <CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void DebugValues()
    {
        if (!_debugValues) return;

        Debug.Log("Player speed magnitud: " + _playerRigidBody.velocity.magnitude);
        Debug.Log("FOV: " + _currentFOV + " Amp: " + _currentAmp + " Freq: " + _currentFreq);
    }
}
