//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject _lobbyLayout;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private List<PlayerLobbyUI> _playerLobbyUIs;
    [SerializeField] private List<Sprite> _playerImages;


    void Start()
    {
        LobbyController.Instance.OnPlayerEnterRoom += JoinPlayerUI;
        LobbyController.Instance.OnFinishedCreateLobby += JoinPlayerUI;
    }

    void Update()
    {

    }


    void JoinPlayerUI(string playerName)
    {
        GameObject playerLobby = Instantiate(_lobbyLayout, transform);
        playerLobby.GetComponent<PlayerLobbyUI>().SetPlayerInfo(playerName, SelectRandomImagePlayer());
    }

    
   Sprite SelectRandomImagePlayer()
    {
        int randomIndex = Random.Range(0, _playerImages.Count);
        Sprite randomImage = _playerImages[randomIndex]; 
        return randomImage;
    }
}
