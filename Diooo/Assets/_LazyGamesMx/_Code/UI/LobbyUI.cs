//Dino 05/04/2023 Creation of the script
//This script control the number of players that are in the lobby and their behavior

using System;
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
        [SerializeField] private GameObject lobbyLayoutParent;
        [SerializeField] private GameObject playerUIPrefab;
        [SerializeField] private List<Sprite> playerImages;

        [SerializeField] private Button startGameButton;

        private NetworkList<PlayerLobbyData> _playersLobbyDatas;


        #endregion

        #region public variables

        
        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            _playersLobbyDatas = new NetworkList<PlayerLobbyData>();

        }

        void Start()
        {
            startGameButton.gameObject.SetActive(false);
            LobbyController.Instance.OnFinishedCreateLobby += SaveLobbyPlayerData;
            LobbyController.Instance.OnClientEnterRoom += SaveLobbyPlayerData;

           
            
        }

        void OnDestroy()
        {
            LobbyController.Instance.OnFinishedCreateLobby -= SaveLobbyPlayerData;
            LobbyController.Instance.OnClientEnterRoom -= SaveLobbyPlayerData;
            

        }
        void Update()
        {

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            LobbyController.Instance.OnFinishedCreateLobby += SaveLobbyPlayerData;
            LobbyController.Instance.OnClientEnterRoom += SaveLobbyPlayerData;
           
            if (IsServer)
            {
                // NetworkManager.Singleton.OnClientConnectedCallback += JoinClientUpdate;
            } 
            
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

        
        
        
        void JoinPlayer()
        {
            UploadLobbyCode();
            UpdatePlayerCount();
            SpawnPlayersInRoom();
        }

        // void JoinClientUpdate(ulong clientId)
        // {
        //     SpawnPlayersInRoom();
        // }
        //     

        void SpawnPlayerUI(PlayerLobbyData playerLobbyData)
        {
            GameObject playerLobby = Instantiate(playerUIPrefab);
            NetworkObject networkObject = playerLobby.GetComponent<NetworkObject>();
            networkObject.Spawn(true);
            playerLobby.transform.SetParent(lobbyLayoutParent.transform);
            playerLobby.gameObject.name = playerLobbyData.ClientId.ToString();
            playerLobby.GetComponent<PlayerLobbyUI>().SetPlayerData(playerLobbyData, SelectImagePlayer(playerLobbyData.PlayerImageIndex));
            
        }
        
        void SpawnPlayersInRoom()
        {
            Debug.Log("SpawnPlayersInRoom");
            if (_playersLobbyDatas.Count == 1)
            {
                SpawnPlayerUI(_playersLobbyDatas[0]);
            }
            else
            {
                foreach (PlayerLobbyData playerLobbyData in _playersLobbyDatas)
                {
                    ulong clientID = playerLobbyData.ClientId;
                    Debug.Log("Player to spawn" + clientID);
                    if(clientID == NetworkManager.Singleton.LocalClientId) return;
                    SpawnPlayerUI(playerLobbyData);
                }
            }
           
        }
        
        
       private int AssignRandomImagePlayer()
        {
            int randomIndex = Random.Range(0, playerImages.Count);
            return randomIndex;
        }
       
       public Sprite SelectImagePlayer(int index)
       {
           Sprite selectedImage = playerImages[index];
           return selectedImage;
       }


       private void SaveLobbyPlayerData(PlayerLobbyData playerData)
       {
           if (IsServer)
           {
               _playersLobbyDatas.Add(new PlayerLobbyData
               {
                   PlayerName = playerData.PlayerName,
                   PlayerImageIndex = AssignRandomImagePlayer(),
                   PlayerId = playerData.PlayerId,
                   ClientId = playerData.ClientId
                
               }); 
           }
          
           Debug.Log("Players DatasList" + _playersLobbyDatas.Count);
           
           JoinPlayer();

       }
       
       #endregion

    }
}
