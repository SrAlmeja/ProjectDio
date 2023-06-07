using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luces : MonoBehaviour
{
    private GameObject luces;
    private bool turnOn;

    private void Awake()
    {
        turnOn = false;
        luces.SetActive(false);
    }

    private void Update()
    {
        if (turnOn)
        {
            luces.SetActive(true);
        }
        else
        {
            luces.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turnOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            turnOn = false;
        }
    }
}
