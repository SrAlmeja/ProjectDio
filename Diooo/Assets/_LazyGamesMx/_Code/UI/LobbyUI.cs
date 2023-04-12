//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace com.LazyGames.Dio
{

    public class LobbyUI : MonoBehaviour
    {
        #region private Variables

        [SerializeField] private Text lobbyCodeText;
        [SerializeField] private GameObject lobbyLayoutParent;
        [SerializeField] private GameObject playerUIPrefab;
        [SerializeField] private List<PlayerLobbyUI> playerLobbyUIs = new List<PlayerLobbyUI>();
        [SerializeField] private List<Sprite> playerImages;

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
            SceneController.Instance.LoadSceneNetwork(SceneKeys.GAME_SCENE);
        }


        #endregion


        #region private methods

        void UploadLobbyCode()
        {
            string lobbyCode = LobbyController.Instance.GetLobby().LobbyCode;
            string lobbyName = LobbyController.Instance.GetLobby().Name;
            lobbyCodeText.text = lobbyName + " CODE =  " + lobbyCode;
        }

        void JoinPlayerUI(string playerName)
        {
            GameObject playerLobby = Instantiate(playerUIPrefab, lobbyLayoutParent.transform);
            playerLobby.GetComponent<PlayerLobbyUI>().SetPlayerInfo(playerName, SelectRandomImagePlayer());
            playerLobbyUIs.Add(playerLobby.GetComponent<PlayerLobbyUI>());
            UploadLobbyCode();
        }

        Sprite SelectRandomImagePlayer()
        {
            int randomIndex = Random.Range(0, playerImages.Count);
            Sprite randomImage = playerImages[randomIndex];
            return randomImage;
        }

        #endregion

    }
}
