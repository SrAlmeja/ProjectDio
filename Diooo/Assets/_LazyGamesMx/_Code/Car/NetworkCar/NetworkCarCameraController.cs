using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;

public class NetworkCarCameraController : NetworkBehaviour
{
    [SerializeField] NetworkCameraFollow networkCameraFollow;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        FindCamera();
    }
    private void FindCamera()
    {
        if (!IsOwner) return;
        Debug.Log("<color=#DDABFF> FindCamera in scene </color>");
        networkCameraFollow = FindObjectOfType<NetworkCameraFollow>();
        networkCameraFollow.SetTarget(transform);
    }
    
    
    
}
