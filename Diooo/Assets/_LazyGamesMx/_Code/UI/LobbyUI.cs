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
        
        private string _myplayerName = "Player";
        private Sprite _myplayerImage;
        private int _myplayerImageIndex;
        private string _myPlayerId;
        private PlayerLobbyData _myPlayerData;
        public NetworkList<PlayerLobbyData> PlayersLobbyDatas;
        
        #endregion

        #region public variables

        public PlayerLobbyData MyPlayerData
        {
            get
            {
                return _myPlayerData;
            }
            set
            {
                _myPlayerData = value;
            }
        }

        public string MyplayerName => _myplayerName;
        public Sprite MyplayerImage
        {
            get
            {
                if (!IsServer)
                {
                    _myplayerImage = SelectImagePlayer(_myplayerImageIndex);
                }
                return _myplayerImage;
            }
        }
        
        public string MyPlayerId => _myPlayerId;

        #endregion
        
        #region Unity Methods

        private void Awake()
        {
            PlayersLobbyDatas = new NetworkList<PlayerLobbyData>();

        }

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
            DioGameMultiplayer.Instance.OnStartHost -= SaveLobbyPlayerData;

        }
        void Update()
        {

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            LobbyController.Instance.OnFinishedCreateLobby += JoinPlayerUI;
            LobbyController.Instance.OnPlayerEnterRoom += JoinPlayerUI;
            if (IsServer)
            {
                DioGameMultiplayer.Instance.OnStartHost += SpawnPlayerUI;
                DioGameMultiplayer.Instance.OnStartHost += SaveLobbyPlayerData;
                NetworkManager.Singleton.OnClientConnectedCallback += JoinClientUpdate;
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

        void JoinPlayerUI(PlayerLobbyData playerData)
        {
            Debug.Log("<color=#7AFFD5>Se crea lobby antes de iniciar host</color>");
            _myPlayerData = playerData;
            _myplayerName = playerData.PlayerName.Value;
            _myplayerImage = SelectRandomImagePlayer();
            _myPlayerId = playerData.PlayerId.Value;
            
            if (IsServer)
            {
                SaveLobbyPlayerData();
            }
            
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
            
        }
        
        void SpawnPlayersInRoom()
        {
            for (int i = 0; i <NetworkManager.Singleton.ConnectedClientsList.Count - 1 ; i++)
            {
                if (!IsServer)
                {
                    MyPlayerData = PlayersLobbyDatas[i];
                    SpawnPlayerUI();

                }
            }
        }
        
        
       private Sprite SelectRandomImagePlayer()
        {
            int randomIndex = Random.Range(0, playerImages.Count);
            Sprite randomImage = playerImages[randomIndex];
            _myPlayerData.PlayerImageIndex = randomIndex;
            
            return randomImage;
        }
       
       private Sprite SelectImagePlayer(int index)
       {
           Sprite randomImage = playerImages[index];
           return randomImage;
       }


       private void SaveLobbyPlayerData()
       {
           PlayerLobbyData newPlayerData = new PlayerLobbyData
           {
               PlayerName = _myPlayerData.PlayerName.Value,
               PlayerImageIndex = _myPlayerData.PlayerImageIndex,
               PlayerId = _myPlayerData.PlayerId.Value
           };
           
           Debug.Log("<color=#7AFFD5>SaveLobbyPlayerData</color>" + newPlayerData.PlayerName.Value +"<color=#7AFFD5>Image index </color>" +  newPlayerData.PlayerImageIndex +"<color=#7AFFD5> Id </color>"  +newPlayerData.PlayerId.Value);
           PlayersLobbyDatas.Add(newPlayerData);
           Debug.Log(PlayersLobbyDatas.Count);

       }
       
       #endregion

    }
}
