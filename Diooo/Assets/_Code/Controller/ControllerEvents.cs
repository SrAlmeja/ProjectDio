using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerEvents : MonoBehaviour
{
    public UnityEvent ControllerEvent;

    private void FixedUpdate()
    {
        ControllerEvent?.Invoke();
    }
}
