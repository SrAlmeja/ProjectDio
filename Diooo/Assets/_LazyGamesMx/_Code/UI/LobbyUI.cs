//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    #region private Variables

    
    [SerializeField] private Text _lobbyCodeText;
    [SerializeField] private GameObject _lobbyLayoutParent;
    [SerializeField] private GameObject _playerUIPrefab;
    [SerializeField] private List<PlayerLobbyUI> _playerLobbyUIs = new List<PlayerLobbyUI>();
    [SerializeField] private List<Sprite> _playerImages;
    #endregion


    #region Unity Methods

    void Start()
    {
        LobbyController.Instance.OnFinishedCreateLobby += JoinPlayerUI;
        LobbyController.Instance.OnPlayerEnterRoom += JoinPlayerUI;
        
    }

    void Update()
    {

    }
    #endregion

    #region public methods
    public void GoToMatch()
    {
        SceneController.Instance.LoadScene(SceneKeys.GAME_SCENE);
    }
    

    #endregion


    #region private methods
    
    void UploadLobbyCode()
    {
        string lobbyCode = LobbyController.Instance.GetLobby().LobbyCode;
        string lobbyName = LobbyController.Instance.GetLobby().Name;
        _lobbyCodeText.text = lobbyName +" CODE =  "+lobbyCode;
    }
    void JoinPlayerUI(string playerName)
    {
        GameObject playerLobby = Instantiate(_playerUIPrefab, _lobbyLayoutParent.transform);
        playerLobby.GetComponent<PlayerLobbyUI>().SetPlayerInfo(playerName, SelectRandomImagePlayer());
        _playerLobbyUIs.Add(playerLobby.GetComponent<PlayerLobbyUI>());
        UploadLobbyCode();
    }
    
   Sprite SelectRandomImagePlayer()
    {
        int randomIndex = Random.Range(0, _playerImages.Count);
        Sprite randomImage = _playerImages[randomIndex]; 
        return randomImage;
    }
   #endregion

}
