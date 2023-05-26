//Dino 05/04/2023 Creation of the script
//This script manage info of the player that is in the lobby

using System;
using com.LazyGames.Dio;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyCar : NetworkBehaviour
{
    #region public variables

    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private GameObject[] cars;
    #endregion

    #region Unity Methods

    private void Start()
    {
    }
    
    #endregion

    public void SetPlayerData(PlayerLobbyData playerLobbyData, int playerCar)
    {
        _playerNameText.text = playerLobbyData.PlayerName.Value;
        cars[playerCar].SetActive(true);
        
        SendPlayerDataClientRpc(playerLobbyData, playerCar);
        
        
    }
    
    [ClientRpc]
    public void SendPlayerDataClientRpc(PlayerLobbyData playerLobbyData, int playerCar)
    {
        Debug.Log("SendPlayerDataClientRpc");
        _playerNameText.text = playerLobbyData.PlayerName.Value;
        cars[playerCar].SetActive(true);
    }

   
}
