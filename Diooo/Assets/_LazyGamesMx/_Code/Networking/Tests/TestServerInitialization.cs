using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestServerInitialization : MonoBehaviour
{
    
    [SerializeField] int numberOfPlayers = 2;
    
    
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnStartTestServer();
        }
    }

    private void OnStartTestServer()
    {
            
    }
    
}
