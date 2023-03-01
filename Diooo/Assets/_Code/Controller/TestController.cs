using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    UnityEvent InputEvent;

    [SerializeField] private float moveSpeed = 5f; // movement speed of the player
    private Rigidbody rb; // rigidbody component of the player object
    public InputAction movementInput; // vector for movement input

    private void OnEnable()
    {
        movementInput.Enable();
    }

    private void OnDisable()
    {
        movementInput.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // get the rigidbody component
    }

    private void FixedUpdate()
    {
        rb.velocity = movementInput.ReadValue<Vector2>() * moveSpeed; // apply movement to the rigidbody
    }
}
