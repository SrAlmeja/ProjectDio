//Dino 05/04/2023 Creation of the script
//This script manage info of the player that is in the lobby
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerLobbyUI : MonoBehaviour
{
    #region public variables

    [SerializeField] private Text _playerNameText;
    [SerializeField] private Image _playerImage;
    

    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        
    }   
    #endregion

    public void SetPlayerInfo(string playerName, Sprite playerImage)
    {
        _playerNameText.text = playerName;
        _playerImage.sprite = playerImage;
    }
        
}
