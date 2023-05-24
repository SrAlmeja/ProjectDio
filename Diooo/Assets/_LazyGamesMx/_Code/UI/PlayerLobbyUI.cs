//Dino 05/04/2023 Creation of the script
//This script manage info of the player that is in the lobby

using System;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyUI : NetworkBehaviour
{
    #region public variables

    [SerializeField] private Text _playerNameText;
    [SerializeField] private Image _playerImage;
    #endregion

    #region Unity Methods

    private void Start()
    {
    }
    
    #endregion

    public void SetPlayerData(PlayerLobbyData playerLobbyData , Sprite playerImage)
    {
        _playerNameText.text = playerLobbyData.PlayerName + " " + playerLobbyData.PlayerId;
        _playerImage.sprite = playerImage;
    }
    

   
}
