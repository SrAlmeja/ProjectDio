//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior

using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace com.LazyGames.Dio
{

    public class LobbyUI : NetworkBehaviour
    {
        #region private Variables

        [SerializeField] private Text lobbyCodeText;
        [SerializeField] private Text playerCountText;
        [SerializeField] private GameObject playerUIPrefab;
        [SerializeField] private List<GameObject> spawnPoints;

        [SerializeField] private Button startGameButton;

        // private NetworkList<PlayerLobbyData> _playersLobbyDatas;
        
        // PlayerLobbyData _clientPlayerLobbyData;
        // private int _clientsIndex;
        private int _spawnPointIndex;
        private int _currentConnectedPlayers;


        #endregion

        #region public variables

        
        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            // _playersLobbyDatas = new NetworkList<PlayerLobbyData>();

        }

        void Start()
        {
            startGameButton.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            LobbyController.Instance.OnFinishedCreateLobby -= SaveLobbyPlayerData;
            // LobbyController.Instance.OnClientEnterRoom -= SaveLobbyPlayerData;
            

        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            LobbyController.Instance.OnFinishedCreateLobby += SaveLobbyPlayerData;

            if (IsServer)
            {
               NetworkManager.Singleton.OnClientConnectedCallback += JoinClientUpdate;
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

        
        void JoinPlayer()
        {
            _currentConnectedPlayers = NetworkManager.Singleton.ConnectedClientsIds.Count;
            UploadLobbyCode();
            UpdatePlayerCount();
            SpawnPlayersInRoom();
        }

        void JoinClientUpdate(ulong clientId)
        {
            JoinPlayer();
        }
            

        void SpawnPlayerUI(PlayerLobbyData playerLobbyData)
        {
            _spawnPointIndex++;
            Debug.Log("<color=#CDFF7A>Spawn Index  </color>" +_spawnPointIndex);
            if (_spawnPointIndex > NetworkManager.Singleton.ConnectedClientsIds.Count)
            {
                Debug.Log("<color=#FE4A3B>Not enough spawn points</color>");
                return; 
            }
            GameObject playerLobby = Instantiate(playerUIPrefab);
            playerLobby.transform.position = spawnPoints[_spawnPointIndex - 1].transform.position;
            playerLobby.transform.rotation = spawnPoints[_spawnPointIndex - 1].transform.rotation;
            NetworkObject networkObject = playerLobby.GetComponent<NetworkObject>();
            networkObject.Spawn(true);
            playerLobby.gameObject.name = playerLobbyData.ClientId.ToString();
            playerLobby.GetComponent<PlayerLobbyCar>().SetPlayerData(playerLobbyData, playerLobbyData.PlayerCarIndex);
            
        }
        
        void SpawnPlayersInRoom()
        {
            Debug.Log("SpawnPlayersInRoom");
            Debug.Log(NetworkManager.Singleton.ConnectedClientsIds.Count + " Clients Connected");

            if (IsServer)
            { 
                SpawnPlayerUI(GenerateRandomPlayerData());
            }
            
            
        }
        
        
        private int AssignRandomCarPlayer()
        {
            int randomIndex = Random.Range(0, 3);
            return randomIndex;
        }

        private string AssignRandomNamePlayer()
        {
            int randomIndex = Random.Range(0, 4);
            string[] names = {"Erduado", "Nico", "Banyo", "Kat", "Mircha"};
            return names[randomIndex];
        }
       
       private void SaveLobbyPlayerData(PlayerLobbyData playerData)
       {

           PlayerLobbyData playerLobbyData = new PlayerLobbyData
           {
               PlayerName = AssignRandomNamePlayer(),
               PlayerCarIndex = AssignRandomCarPlayer(),
               PlayerId = playerData.PlayerId,
               ClientId = playerData.ClientId
           };
           
           // _clientPlayerLobbyData = playerLobbyData;
           JoinPlayer();
       }

       private PlayerLobbyData GenerateRandomPlayerData()
       {
           PlayerLobbyData playerLobbyData = new PlayerLobbyData
           {
               PlayerName = AssignRandomNamePlayer(),
               PlayerCarIndex = AssignRandomCarPlayer(),
               PlayerId = "",
               ClientId = 0
           };
           return playerLobbyData;
       }
       
        #endregion

        
    }
}
