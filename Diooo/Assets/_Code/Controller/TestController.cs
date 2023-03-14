//Leinad 29/02/23
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    private PlayerInput _playerInputActions;
    private InputAction _movement;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        _movement = _playerInputActions.CarController.Move;
        _movement.Enable();
    }

    private void OnDisable()
    {
        _movement.Disable();
    }

    public void Movement()
    {
        Debug.Log("mocement values" + _movement.ReadValue<Vector2>());
    }


}
