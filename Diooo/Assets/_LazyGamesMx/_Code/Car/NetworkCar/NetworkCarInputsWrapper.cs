using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using CryoStorage;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkCarInputsWrapper : NetworkBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    InputActionMap gameplayActionMap;

    private InputAction _handBrakeInputAction;
    private InputAction _steeringInputAction;
    private InputAction _leftTriggerInputAction;
    private InputAction _rightTriggerInputAction;
    private InputAction _timeStopInputAction;
    private InputAction _rotateInputAction;
    private InputAction _impulseInputAction;

    [SerializeField] private BoolEventChannelSO handbrakeEvent;
    [SerializeField] private VoidEventChannelSO stopTimeEvent;
    [SerializeField] private FloatEventChannelSO angleEvent;
    [SerializeField] private FloatEventChannelSO lTEvent;
    [SerializeField] private FloatEventChannelSO rTEvent;
    [SerializeField] private FloatEventChannelSO rotateEvent;
    [SerializeField] private VectorTwoEventChannelSO vector2InputEvent;
    [SerializeField] private VoidEventChannelSO impulseEvent;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        PrepareInputs();
        
        _handBrakeInputAction.Enable();
        _steeringInputAction.Enable();
        _leftTriggerInputAction.Enable();
        _rightTriggerInputAction.Enable();
        _timeStopInputAction.Enable();
        _rotateInputAction.Enable();
        _impulseInputAction.Enable();
        

    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        
        _handBrakeInputAction.Disable();
        _steeringInputAction.Disable();
        _leftTriggerInputAction.Disable();
        _rightTriggerInputAction.Disable();
        _timeStopInputAction.Disable();
        _rotateInputAction.Disable();
        _impulseInputAction.Disable();
    }

    void PrepareInputs()
    {
        gameplayActionMap = inputActions.FindActionMap("Gameplay");

        _handBrakeInputAction = gameplayActionMap.FindAction("HandBrake");
        _steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
        _leftTriggerInputAction = gameplayActionMap.FindAction("LeftTrigger");
        _rightTriggerInputAction = gameplayActionMap.FindAction("RightTrigger");
        _timeStopInputAction = gameplayActionMap.FindAction("TimeStop");
        _rotateInputAction = gameplayActionMap.FindAction("Rotate");
        _impulseInputAction = gameplayActionMap.FindAction("Impulse");

        _handBrakeInputAction.performed += GetHandBrakeInput;
        _handBrakeInputAction.canceled += GetHandBrakeInput;

        _steeringInputAction.performed += GetAngleInput;
        _steeringInputAction.canceled += GetAngleInput;
            
        _leftTriggerInputAction.performed += GetLtInput;
        _leftTriggerInputAction.canceled += GetLtInput;
    
        _rightTriggerInputAction.performed += GetRtInput;
        _rightTriggerInputAction.canceled += GetRtInput;
    
        _timeStopInputAction.performed += StopTimeInput;
        _timeStopInputAction.canceled += StopTimeInput;
    
        _rotateInputAction.performed += RotateInput;
        _rotateInputAction.canceled += RotateInput;
    
        _impulseInputAction.performed += GetImpulseInput;
        _impulseInputAction.canceled += GetImpulseInput;
    }
    
    
    void GetHandBrakeInput(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() == 0)
        {
            handbrakeEvent.RaiseEvent(false);
            Debug.Log("isfalse");
        }
        else
        {
            handbrakeEvent.RaiseEvent(true);
            Debug.Log("istrue");
        }  
    }
    
    void GetAngleInput(InputAction.CallbackContext context)
    {
        angleEvent.RaiseEvent(context.ReadValue<float>());
    }
    void GetRtInput(InputAction.CallbackContext context)
    {
        rTEvent.RaiseEvent(context.ReadValue<float>());
    }
        
    void GetLtInput(InputAction.CallbackContext context)
    {
        lTEvent.RaiseEvent(context.ReadValue<float>());
    }

    void StopTimeInput(InputAction.CallbackContext context) 
    {
        stopTimeEvent.RaiseEvent();
    }

    void RotateInput(InputAction.CallbackContext context)
    {
        Vector2 vectorInput = context.ReadValue<Vector2>();
        float angle = CryoMath.AngleFromOffset(vectorInput);
        rotateEvent.RaiseEvent(angle);
        vector2InputEvent.RaiseEvent(context.ReadValue<Vector2>());
    }

    void GetImpulseInput(InputAction.CallbackContext context)
    {
        impulseEvent.RaiseEvent();
    }
}
