//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior
// Script already works with the new lobby system

using System.Collections.Generic;
using com.LazyGames.Dio;
using Mono.CSharp;
using TMPro;
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

        [SerializeField] private TMP_Text lobbyCodeText;
        [SerializeField] private TMP_Text playerCountText;
        [SerializeField] private GameObject playerUIPrefab;
        [SerializeField] private List<GameObject> spawnPoints;
        [SerializeField] private GameObject startGameTxt;
        [SerializeField] private GoToMatchInput goToMatchInput;

    private int _spawnPointIndex;
        private bool _canStartMatch;
        #endregion

        #region public variables

        public bool CanStartMatch => _canStartMatch;
        #endregion
        
        #region Unity Methods


        void Start()
        {
            startGameTxt.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            LobbyController.Instance.OnFinishedCreateLobby -= SaveLobbyPlayerData;
            NetworkManager.Singleton.OnClientConnectedCallback -= JoinClientUpdate;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            LobbyController.Instance.OnFinishedCreateLobby += SaveLobbyPlayerData;

            if (IsServer)
            {
               NetworkManager.Singleton.OnClientConnectedCallback += JoinClientUpdate;
            }
                
        }

        #endregion

        #region public methods

        // public void GoToMatch()
        // {
        //     SceneController.Instance.LoadSceneNetwork(SceneKeys.GAME_SCENE);
        // }

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
            UploadLobbyCode();
            UpdatePlayerCount();
            SpawnPlayersInRoom();
        }

        void JoinClientUpdate(ulong clientId)
        {
            UpdatePlayersSpawned();
            JoinPlayer();
            if (NetworkManager.Singleton.ConnectedClientsIds.Count >= 2)
            {
                startGameTxt.gameObject.SetActive(true);
                _canStartMatch = true;
                if(IsServer)
                    goToMatchInput.PrepareInputs();
            }
        }
            
        
        void SpawnPlayerUI(PlayerLobbyData playerLobbyData)
        {
            _spawnPointIndex++;
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

        private void UpdatePlayersSpawned()
        {
            PlayerLobbyCar[] playerLobbyCars = FindObjectsOfType<PlayerLobbyCar>();
            foreach (PlayerLobbyCar playerLobbyCar in playerLobbyCars)
            {
                playerLobbyCar.SendPlayerDataClientRpc(playerLobbyCar.PlayerLobbyData,
                    playerLobbyCar.PlayerLobbyData.PlayerCarIndex);
            }
        }
        void SpawnPlayersInRoom()
        {
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
