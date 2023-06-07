using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{

    [SerializeField] private Maniquis[] listOfScripts;
    private Maniquis _maniquis;
    

    private void Start()
    {
        foreach (Maniquis maniquis in listOfScripts)
        {
            _maniquis = GetComponent<Maniquis>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Maniquis maniquis in listOfScripts)
            {
                maniquis.isInRange = true;
            }
            Debug.Log("Jugador entro");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Maniquis maniquis in listOfScripts)
            {
                _maniquis.isInRange = false;
            }
            Debug.Log("Jugador salio");
        }
    }
}
