using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Grid : MonoBehaviour
{
    private int width, height;
    private float cellSize;
    private int[,] gridArray;
    
    
    public Grid(int _width, int _height, float _cellSize, GameObject prefab)
    {
        this.width = _width;
        this.height = _height;
        this.cellSize = _cellSize;

        gridArray = new int[_width, _height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.cyan, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.cyan, 100f);
                
                GameObject cellBox = Instantiate(prefab);
                cellBox.name = $"{x}-{y}";
                
                prefab.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                
            }
        }
        Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.cyan, 100f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.cyan, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y > height)
        {
            gridArray[x, y] = value;
        }
    }
}
