using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private int x, y;
    [SerializeField] private float cellSize;
    public GameObject emptyObjectTest;
   
    
    // Start is called before the first frame update
    void Start()
    {
        Grid grid = new Grid(x,y, cellSize, emptyObjectTest);
    }

}
