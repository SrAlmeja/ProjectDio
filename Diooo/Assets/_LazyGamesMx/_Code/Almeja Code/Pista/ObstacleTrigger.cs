using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{

    [SerializeField] private Maniquis[] maniquies;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player se metio");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player se salio");
    }
}
