using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMDioCameraController : MonoBehaviour
{
    [Header("Dependenices")]
    [SerializeField] private CinemachineFreeLook _cmVirtualCamera;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _playerRigidBody;

    [Header("Settings")]
    [SerializeField] private float _maxSpeed = 1f;

    [Header("Debug")]
    [SerializeField] private bool _debugValues = false;

    private CinemachineBasicMultiChannelPerlin[] _cmRigPerlinNoise = new CinemachineBasicMultiChannelPerlin[3];

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

    private void OnEnable()
    {
        CheckDependencies();
        GetRigs();
    }

    private void Update()
    {
        _currentSpeed = SpeedPercentage(); //Always leave this on top. It calculates % of the max speed. 
        ChangeFOV();
        ChangeShake();
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

    private float SpeedPercentage()
    {
        float speedPercentage = Mathf.Clamp01(_playerRigidBody.velocity.magnitude / _maxSpeed);
        return speedPercentage;
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
