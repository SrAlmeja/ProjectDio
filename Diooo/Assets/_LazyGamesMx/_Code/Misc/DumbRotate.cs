using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbRotate : MonoBehaviour
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed = .3f;
    [SerializeField] private float zSpeed;

    void Update()
    {
        Vector3 rot = new Vector3(xSpeed, ySpeed, zSpeed);
        transform.Rotate(rot * Time.deltaTime);
    }
}
