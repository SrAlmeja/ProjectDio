//Raymundo cryoStorage Mosqueda 07/03/2023
//

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Car_TimeControl : MonoBehaviour
{
    [Header("TimeControl")] 
    [SerializeField] private bool doSlow;
    [FormerlySerializedAs("timeScale")] [SerializeField] private float mytimeScale;

    private float savedMagnitude;
    private Rigidbody _rb;

    private void Start()
    {
        Prepare();
    }

    private void Update()
    {
        Time.timeScale = mytimeScale;
        if (Input.GetKeyDown(KeyCode.K))
            mytimeScale = Mathf.Epsilon> Mathf.Abs(mytimeScale-.1f) ? 1 : .1f;
        Slow();
        // _rb.velocity = _rb.velocity * mytimeScale;
    }

    void Slow()
    {
        if (!doSlow) return;
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
        
        // Time.timeScale
    }

    private void SaveVelocity()
    {
        savedMagnitude = _rb.velocity.magnitude;
    }

    private void ChangeTimeScale(float value)
    {
        mytimeScale = value;
    }

    void Prepare()
    {
        _rb = GetComponent<Rigidbody>();

    }
}
