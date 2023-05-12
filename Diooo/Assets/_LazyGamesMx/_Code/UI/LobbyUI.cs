//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace com.LazyGames.Dio
{

    public class LobbyUI : NetworkBehaviour
    {
        #region private Variables

        [SerializeField] private Text lobbyCodeText;
        [SerializeField] private Text playerCountText;
        [SerializeField] private GameObject lobbyLayoutParent;
        [SerializeField] private GameObject playerUIPrefab;
        [SerializeField] private List<PlayerLobbyUI> playerLobbyUIs = new List<PlayerLobbyUI>();
        [SerializeField] private List<Sprite> playerImages;

        [SerializeField] private Button startGameButton;
        #endregion


        #region Unity Methods

        void Start()
        {
            startGameButton.gameObject.SetActive(false);
            LobbyController.Instance.OnFinishedCreateLobby += JoinPlayerUI;
            LobbyController.Instance.OnPlayerEnterRoom += JoinPlayerUI;
        }

        void Update()
        {

        }

        public override void OnNetworkSpawn()
        {
            LobbyController.Instance.OnFinishedCreateLobby += JoinPlayerUI;
            LobbyController.Instance.OnPlayerEnterRoom += JoinPlayerUI;

            if (IsServer)
            {
                startGameButton.gameObject.SetActive(true);
            }
                
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
        void UpdatePlayerCount()
        {
            int playerCount = LobbyController.Instance.GetLobby().Players.Count;
            playerCountText.text = playerCount + " / " + LobbyController.Instance.GetLobby().MaxPlayers;
        }

        void JoinPlayerUI(string playerName)
        {
            GameObject playerLobby = Instantiate(playerUIPrefab, lobbyLayoutParent.transform);
            playerLobby.GetComponent<PlayerLobbyUI>().SetPlayerInfo(playerName, SelectRandomImagePlayer());
            playerLobbyUIs.Add(playerLobby.GetComponent<PlayerLobbyUI>());
            UploadLobbyCode();
            UpdatePlayerCount();
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
