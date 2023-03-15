using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour
{
    public FloatVoidEvent HanbreakeEvent;

    private void OnEnable()
    {
        HanbreakeEvent.OnEventRaised += HandBrake;
    }

    private void OnDisable()
    {
        HanbreakeEvent.OnEventRaised -= HandBrake;
    }

    void HandBrake(float f)
    {
        Debug.Log("Si funciona :D: " + f);
    }
}
