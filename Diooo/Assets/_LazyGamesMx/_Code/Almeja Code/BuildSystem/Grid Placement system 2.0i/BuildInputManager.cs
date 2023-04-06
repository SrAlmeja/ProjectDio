using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildInputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    
    public Vector3 lastposition;
    
    [SerializeField]
    private LayerMask placementLayermask;

    public event Action Onclicked, OnExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Onclicked.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        
        //Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(sceneCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, placementLayermask))
        {
            mousePos.y = sceneCamera.nearClipPlane;
            lastposition = hit.point;
        }

        return lastposition;
    }
}
