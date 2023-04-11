using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector3 mousePosition = Input.mousePosition;
        transform.position = mousePosition;
    }
    
}
