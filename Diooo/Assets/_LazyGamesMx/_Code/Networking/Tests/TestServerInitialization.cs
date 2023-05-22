using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestServerInitialization : MonoBehaviour
{
    
    [SerializeField] int numberOfPlayers = 2;
    [SerializeField] bool isTestingActive = false;
    
    
    
    void Start()
    {
        if (isTestingActive)
        {
            OnStartTestServer();
        }
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     OnStartTestServer();
        // }
    }

    private void OnStartTestServer()
    { 
        NetworkManager.Singleton.StartHost();
    }
    
    
    
}
