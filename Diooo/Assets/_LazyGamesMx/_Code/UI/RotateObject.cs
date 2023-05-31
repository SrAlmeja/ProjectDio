using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public GameObject objectToRotate;
    public float rotationSpeed;
    
    void Update()
    {
        DoConstantRotation();
    }

    private void DoConstantRotation()
    {
        objectToRotate.transform.Rotate(Vector3.up * (Time.deltaTime * rotationSpeed));
    }
}
