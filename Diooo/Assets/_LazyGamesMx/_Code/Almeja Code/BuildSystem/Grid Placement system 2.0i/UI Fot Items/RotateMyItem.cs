using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateMyItem : MonoBehaviour
{
    [SerializeField] private Canvas buttonCanvas;
    
    private float left = -45;
    private float right = 45;

    

    public void LeftRotation()
    {
        transform.Rotate(0,0,left);
    }
    public void rightRotation()
    {
        transform.Rotate(0,0,right);
    }
}
