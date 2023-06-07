using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float x, y, z; 
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(x,y,z);
    }
}
