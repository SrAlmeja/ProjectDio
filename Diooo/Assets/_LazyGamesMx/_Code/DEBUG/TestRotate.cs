using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;
using CryoStorage;

public class TestRotate : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private float impulseRadius = 4.5f;
    [SerializeField] private float angle = 0;
    private DebugSteeringEventsListener _listener;
    
    // Start is called before the first frame update
    void Start()
    {
        _listener = GetComponent<DebugSteeringEventsListener>();
    }

    // Update is called once per frame
    void Update()
    {
        angle = CryoMath.AngleFromOffset(_listener.Vec2Input);
        indicator.transform.position = CryoMath.PointOnRadius(transform.position, impulseRadius, angle);
        indicator.transform.rotation = CryoMath.AimAtDirection(transform.position, indicator.transform.position);
        Debug.Log(CryoMath.AngleFromOffset(_listener.Vec2Input));    
    }
    
}
