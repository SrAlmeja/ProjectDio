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
    [SerializeField] private float targetTimeScale;
    
    
    private float customTimeScale;
    private float savedMagnitude;
    private Rigidbody rb;
    private readonly float deltaTimeNormalizer = .02f;

    private void Start()
    {
        Prepare();
    }

    private void Update()
    {
        Slow();
    }

    void Slow()
    {
        // targetTimeScale = Mathf.Epsilon> Mathf.Abs(targetTimeScale-.1f) ? 1 : .01f;
        if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = targetTimeScale;
            Time.fixedDeltaTime = Time.timeScale * deltaTimeNormalizer;
        }
    }

    public void DebuggingInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {  
            switch (doSlow)
            {
                case false:
                    doSlow = true;
                    ChangeTimeScale(1f);
                    break;
                case true:
                    doSlow = false;
                    ChangeTimeScale(0f);
                    break;
            }
        }
    }
    
    private void ChangeTimeScale(float value)
    {
        targetTimeScale = value;
    }

    void Prepare()
    {
        rb = GetComponent<Rigidbody>();
    }
}
