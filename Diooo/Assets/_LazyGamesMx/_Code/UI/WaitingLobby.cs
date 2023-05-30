using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;

public class WaitingLobby : MonoBehaviour
{
    
    [SerializeField] private GameObject _waitingLobby;
    void Start()
    {
        _waitingLobby.SetActive(false);
        DioGameMultiplayer.Instance.OnStartClient += UpdateWaitingLobby;
    }
    
    
    private void UpdateWaitingLobby()
    {
        _waitingLobby.SetActive(true);
    }
    
    
    
}
