//Raymundo cryoStorage Mosqueda 07/03/2023
//

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car_TimeControl : MonoBehaviour
{
    [Header("TimeControl")] 
    [SerializeField] private bool doSlow;
    [SerializeField] private float timeScale;

    private float savedMagnitude;
    private Rigidbody _rb;

    private void Start()
    {
        Prepare();
    }

    private void Update()
    {
        Slow();
        _rb.velocity = _rb.velocity * timeScale;
    }

    void Slow()
    {
        if (!doSlow) return;
        _rb.velocity = _rb.velocity * timeScale; 
    }

    public void DebuggingInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {  
            Debug.Log("q press");
            switch (doSlow)
            {
                case false:
                    doSlow = true;
                    SaveVelocity();
                    ChangeTimeScale(1f);
                    break;
                case true:
                    doSlow = false;
                    ChangeTimeScale(0f);
                    break;
            }
        }
    }

    private void SaveVelocity()
    {
        savedMagnitude = _rb.velocity.magnitude;
    }

    private void ChangeTimeScale(float value)
    {
        timeScale = value;
    }

    void Prepare()
    {
        _rb = GetComponent<Rigidbody>();

    }
}
