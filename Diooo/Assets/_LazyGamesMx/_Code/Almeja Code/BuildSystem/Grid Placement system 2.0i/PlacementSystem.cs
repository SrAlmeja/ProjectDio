using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private int _selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    [SerializeField]
    private DataGrid floorData, furnitureData;

    private Renderer _previewRenderer;

    private List<GameObject> placedGameObjects = new(); 

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
        _previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    
    public void StartPlacement(int ID)
    {
        _selectedObjectIndex = oDBSO.objectData.FindIndex(data => data.ID == ID);
        if (_selectedObjectIndex < 0)
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

        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        if (placementValidity == false)
            return;
        
        GameObject newPart= Instantiate(oDBSO.objectData[_selectedObjectIndex].Prefab);
        newPart.transform.position = grid.CellToWorld(gridPosition);
        
        placedGameObjects.Add(newPart);
        
        DataGrid selecteDataGrid = oDBSO.objectData[_selectedObjectIndex].ID == 0 ?
            floorData : furnitureData;
        
        selecteDataGrid.AddObjectAt(gridPosition,
            oDBSO.objectData[_selectedObjectIndex].Size,
            oDBSO.objectData[_selectedObjectIndex].ID,
            placedGameObjects.Count -1);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        DataGrid selecteDataGrid = oDBSO.objectData[selectedObjectIndex].ID == 0 ?
            floorData : furnitureData;
        return selecteDataGrid.CanPlaceObjectAt(gridPosition, oDBSO.objectData[selectedObjectIndex].Size);
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
        if (_selectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = bIM.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        
        bool placementValidity = CheckPlacementValidity(gridPosition, _selectedObjectIndex);
        _previewRenderer.material.color = placementValidity ? Color.white : Color.red;
        
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
