using com.LazyGames.Dio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour
{
    public FloatEventChannelSO HanbreakeEvent;

    private void OnEnable()
    {
        HanbreakeEvent.FloatEvent += HandBrake;
    }

    private void OnDisable()
    {
        HanbreakeEvent.FloatEvent -= HandBrake;
    }

    void HandBrake(float f)
    {
        Debug.Log("Si funciona :D: " + f);
    }
}
