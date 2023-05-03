using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCameraFollow : NetworkBehaviour
{
    
    [SerializeField] private Vector3 offset = new Vector3(0,4,-10);
    [SerializeField] private Transform target;
    [SerializeField] private float translateSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;

    [HideInInspector]public bool HasTarget;

    public void SetTarget(GameObject target)
    {
        this.target = target.transform;
    }

    IEnumerator WaitForTarget()
    {
        yield return new WaitUntil(() =>HasTarget); 
    }
    
    private void FixedUpdate()
    {
        if(!IsOwner) return;
        HandleTranslation();
        HandleRotation();
    }
   
    private void HandleTranslation()
    {
        if(!IsOwner) return;
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        if(!IsOwner) return;
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}