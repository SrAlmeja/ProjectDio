using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] 
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private BuildInputManager bIM;

    [SerializeField]
    private GridLayout grid;
    
    
    [SerializeField]
    private ObjectsDatabaseSO oDBSO;

    private int selectedObjectIndex;

    [SerializeField] private GameObject gridVisualization;

    private void Start()
    {
        StopPlacement();
    }

    
    public void StartPlacement(int ID)
    {
        selectedObjectIndex = oDBSO.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"ID no encontrado {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        bIM.Onclicked += PlaceStructure;
        bIM.OnExit += StopPlacement;
    }
    
    private void PlaceStructure()
    {
        if (bIM.IsPointerOverUI())
        {
            return;
            
        }
        Vector3 mousePosition = bIM.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newPart= Instantiate(oDBSO.objectData[selectedObjectIndex].Prefab);
        newPart.transform.position = grid.CellToWorld(gridPosition);
    }
    
    private void StopPlacement()
    {
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        bIM.Onclicked -= PlaceStructure;
        bIM.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = bIM.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
