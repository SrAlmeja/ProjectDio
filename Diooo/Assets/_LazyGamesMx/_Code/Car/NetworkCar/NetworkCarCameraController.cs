using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;

public class NetworkCarCameraController : NetworkBehaviour
{
    [SerializeField] CMDioCameraController _cameraController;
        
    public override void OnNetworkSpawn()
    {
        FoundCameraInScene();
    }
        
    void FoundCameraInScene()
    {
        if (IsOwner)
        {
            _cameraController = FindObjectOfType<CMDioCameraController>();
            if (_cameraController != null)
            {
                _cameraController.SetTargetNetwork(transform);
                
            }
        }
    }
    
}
