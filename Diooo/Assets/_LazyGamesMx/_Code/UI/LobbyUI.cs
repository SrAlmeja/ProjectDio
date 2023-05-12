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
        [SerializeField] private List<Sprite> playerImages;

        [SerializeField] private Button startGameButton;
        
        private string _myplayerName = "Player";
        // private NetworkList<T> playerLobbyUIs = new NetworkList<>();
        
        private PlayerLobbyData _myPlayerData;
        private NetworkList<PlayerLobbyData> playerLobbyUIs;
        
        #endregion

        #region public variables


        public string MyplayerName => _myplayerName;
        #endregion
        
        #region Unity Methods

        void Start()
        {
            startGameButton.gameObject.SetActive(false);
            LobbyController.Instance.OnFinishedCreateLobby += JoinPlayerUI;
            LobbyController.Instance.OnPlayerEnterRoom += JoinPlayerUI;

           
            
        }

        void OnDestroy()
        {
            LobbyController.Instance.OnFinishedCreateLobby -= JoinPlayerUI;
            LobbyController.Instance.OnPlayerEnterRoom -= JoinPlayerUI;
            DioGameMultiplayer.Instance.OnStartHost -= SpawnPlayerUI;
            DioGameMultiplayer.Instance.OnStartClient -= SpawnPlayerUI;
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
                DioGameMultiplayer.Instance.OnStartHost += SpawnPlayerUI;
                NetworkManager.Singleton.OnClientConnectedCallback += JoinClientUpdate;
            } 
            
            // DioGameMultiplayer.Instance.OnStartClient += SpawnPlayerUI;
            
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

        void JoinPlayerUI(PlayerLobbyData playerData)
        {
            _myPlayerData = playerData;
            _myplayerName = playerData.PlayerName.Value;
            UploadLobbyCode();
            UpdatePlayerCount();

            if (DioGameMultiplayer.Instance.IsHostInitialized.Value)
            {
                Debug.Log("SpawnPlayerUI");
                SpawnPlayerUI();
                SpawnPlayersInRoom();
            }
        }

        void JoinClientUpdate(ulong clientId)
        {
            JoinPlayerUI(_myPlayerData);
        }
            

        void SpawnPlayerUI()
        {
            GameObject playerLobby = Instantiate(playerUIPrefab);
            NetworkObject networkObject = playerLobby.GetComponent<NetworkObject>();
            networkObject.Spawn(true);
            playerLobby.transform.SetParent(lobbyLayoutParent.transform);
            // playerLobbyUIs.Add(playerLobby.GetComponent<PlayerLobbyUI>());
            
        }
        
        void SpawnPlayersInRoom()
        {
            for (int i = 0; i <NetworkManager.Singleton.ConnectedClientsList.Count - 1 ; i++)
            {
                if (!IsServer)
                    SpawnPlayerUI();
            }
        }
        
        
       public Sprite SelectRandomImagePlayer()
        {
            int randomIndex = Random.Range(0, playerImages.Count);
            Sprite randomImage = playerImages[randomIndex];
            return randomImage;
        }

        #endregion

    }
}
