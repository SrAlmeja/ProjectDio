//Dino 05/04/2023 Creation of the script
//This script manage info of the player that is in the lobby

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

    public override void OnNetworkSpawn()
    {
        LobbyUI lobbyUI = FindObjectOfType<LobbyUI>();
        string playerName = lobbyUI.MyplayerName;
        string playerId = lobbyUI.MyPlayerId;
        Sprite playerImage = lobbyUI.MyplayerImage;
        SetPlayerInfo(playerName + " "+ playerId, playerImage);
    }
    
    #endregion

    private void SetPlayerInfo(string playerName, Sprite playerImage)
    {
        _playerNameText.text = playerName;
        _playerImage.sprite = playerImage;
    }

   
}
