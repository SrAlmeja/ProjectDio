using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public static GameObject itemDragging;
    
    private Vector3 startPosition;
    private Transform startParent;
    private Transform dragParent;
    

    private void Start()
    {
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        itemDragging = gameObject;
        startParent = transform.parent;

        startPosition = transform.position;
        
        transform.SetParent(dragParent);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        itemDragging = null;
        if (transform.parent == dragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
            
        }
    }
}
