//Dino 15/03/22  Lobby Connection

using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Collections;
using Unity.Netcode;


namespace com.LazyGames.Dio
{
    public class LobbyController : MonoBehaviour
    {
        
        #region public variables

        public static LobbyController Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject lobbyControllerGO = new GameObject("LobbyController");
                    lobbyControllerGO.SetActive(false);
                    _instance = lobbyControllerGO.AddComponent<LobbyController>();

                    lobbyControllerGO.SetActive(true);
                    DontDestroyOnLoad(lobbyControllerGO);
                    
                }

                return _instance;
            }   
        }
        [SerializeField] string defaultPlayerName = "Player";
        [SerializeField] string lobbyName = "DinoLobby";
        [SerializeField] int maxPlayers = 4;

        public Action<PlayerLobbyData> OnClientEnterRoom;
        public Action<PlayerLobbyData> OnFinishedCreateLobby;
        public Action OnFinishedCheckedLobbies;

        #endregion
        
        #region private variables

        private static LobbyController _instance;
        private Lobby _myJoinedLobby;
        private int _listLobbyCount = 0;
        private float _heartbeatTimer = 0.0f;
        private float _lobbyUpdateTimer = 0.0f;
        
        
        private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
        #endregion

        #region unity methods
        
        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!ConnectionNetworking.Instance.HasInternet)
                return;
            
            
            AuthenticatorController.Instance.OnFinishedAnonymousLogin += ListLobbies;
            OnFinishedCheckedLobbies += CheckedLobbyExists;
            DioGameMultiplayer.Instance.OnStartHost += FinishedCreatedLobby;
            DioGameMultiplayer.Instance.OnStartClient += FinishedJoinedLobby;

        }
        
        private void Update()
        {
            HandleLobbyHeartbeat();
            HandleLobbyPollUpdate();
        }

        private void OnDestroy()
        {
            if(AuthenticatorController.Instance != null)
                AuthenticatorController.Instance.OnFinishedAnonymousLogin -= ListLobbies;
           
            OnFinishedCheckedLobbies -= CheckedLobbyExists;
        }

        #endregion
        
        
        #region Lobby

        public Lobby GetLobby()
        {
            return _myJoinedLobby;
        }
        private async void CreateLobby()
        {
            try
            {
                _myJoinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, new CreateLobbyOptions
               {
                   IsPrivate = false,
               });
                Debug.Log("Created lobby with id: " + _myJoinedLobby.Name + " " + _myJoinedLobby.MaxPlayers + " LOBBY ID =  " + _myJoinedLobby.Id);
                RelayController.Instance.CreateRelayServer(_myJoinedLobby.Id, KEY_RELAY_JOIN_CODE);
                
            }
            catch (LobbyServiceException exception)
            {
                Debug.Log(exception);
            }
        }

        private void FinishedCreatedLobby()
        {
            OnFinishedCreateLobby?.Invoke( new PlayerLobbyData
            {
                PlayerName = GetPlayer().Data["Player Name"].Value,
                PlayerCarIndex = 0,
                PlayerId = GetPlayer().Data["Player Id"].Value,
                ClientId = NetworkManager.Singleton.LocalClientId
            });
        }
        private void FinishedJoinedLobby()
        {
            OnClientEnterRoom?.Invoke( new PlayerLobbyData
            {
                PlayerName = GetPlayer().Data["Player Name"].Value,
                PlayerCarIndex = 0,
                PlayerId = GetPlayer().Data["Player Id"].Value,
                ClientId = NetworkManager.Singleton.LocalClientId
            });
        }
        private async void ListLobbies()
        {
            try
            {
                QueryResponse queryResponse =  await Lobbies.Instance.QueryLobbiesAsync();
                // Debug.Log("Lobbies found: " + queryResponse.Results.Count);
                _listLobbyCount = queryResponse.Results.Count;
                // foreach (Lobby lobby in queryResponse.Results)
                // {
                    // Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
                // }

                OnFinishedCheckedLobbies?.Invoke();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private void CheckedLobbyExists()
        {
            if (_listLobbyCount != 0)
            {
                // Debug.Log("<color=#92FFF0>EXISTE UN LOBBY Y SE PUEDE UNIR </color>");
                QuickJoinLobby();
            }else
            {
                // Debug.Log("<color=#92FFF0>NO EXISTE UN LOBBY Y SE PUEDE CREAR</color>");
                CreateLobby();
            }
           
        }

        // private async void JoinLobbyByCode(string lobbyCode)
        // {
            // try
            // {
                // QueryResponse queryResponse =  await Lobbies.Instance.QueryLobbiesAsync();
                
                // JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
                // {
                    // Player = GetPlayer()
                // };
                
               // Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
               // _myJoinedLobby = lobby;

                // Debug.Log("Joined lobby with code: " + lobbyCode);
                
                // PrintPlayers(lobby);
            // }
            // catch (LobbyServiceException e)
            // {
                // Debug.Log(e);
            // }
            // }

        private async void QuickJoinLobby()
        {
            try
            { 
                _myJoinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                string relayJoinCode = _myJoinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                RelayController.Instance.JoinRelayServer(relayJoinCode);
                
                // Debug.Log("QUICK JOIN LOBBY CODE" + _myJoinedLobby.LobbyCode);
                // OnClientEnterRoom?.Invoke(new PlayerLobbyData
                //     {
                //         PlayerName =  GetPlayer().Data["Player Name"].Value,
                //         PlayerImageIndex = 0,
                //         PlayerId = GetPlayer().Data["Player Id"].Value,
                //         ClientId = NetworkManager.Singleton.LocalClientId
                //     });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                throw;
            }
        }

        // private async void LeaveLobby()
        // {
        //     try
        //     {
        //         await LobbyService.Instance.RemovePlayerAsync(_myJoinedLobby.Id, AuthenticationService.Instance.PlayerId);
        //
        //     }
        //     catch (LobbyServiceException e)
        //     {
        //         Debug.Log(e);
        //         throw;
        //     }
        // }
        //
        //Mostrar info del player en el lobby
        private Player GetPlayer()
        {
            return new Player
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"Player Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, defaultPlayerName)},
                    {"Player Id", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, AuthenticationService.Instance.PlayerId)},
                }
            };
        }
        // private void PrintPlayers(Lobby lobby)
        // {
        //     Debug.Log("Players in lobby: " + lobby.Players.Count); 
        //     foreach (Player player in lobby.Players)
        //     {
        //         Debug.Log("MY PLAYER ID =" +player.Id + " MY PLAYER NAME " + player.Data["Player Name"].Value);  
        //     }
        // }

        // Upadate player Data
         private async void UpdatePlayerName(string newPLayerName)
         {
             try
             {
                 defaultPlayerName = newPLayerName;
                 await LobbyService.Instance.UpdatePlayerAsync(_myJoinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
                 {
                     Data = new Dictionary<string, PlayerDataObject>
                     {
                         {"Player Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, newPLayerName)}
                     }
                 });
        
             }
             catch (LobbyServiceException e)
             {
                 Debug.Log(e);
                 throw;
             }
             
         }
        
        private bool IsLobbyHost()
        {
            return _myJoinedLobby != null && _myJoinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }
        
        // Mantiene abierto el lobby por más de 30 segundos
        private async void HandleLobbyHeartbeat()
        {
            if (IsLobbyHost())
            {
                _heartbeatTimer -= Time.deltaTime;
                if (_heartbeatTimer < 0f)
                {
                    float heartbeatTimerMax = 15f;
                    _heartbeatTimer = heartbeatTimerMax;

                    await LobbyService.Instance.SendHeartbeatPingAsync(_myJoinedLobby.Id);
                }
            }
        }
        
        //Actualiza la info del lobby
        private async void HandleLobbyPollUpdate()
        {
            if(_myJoinedLobby != null)
                _lobbyUpdateTimer -= Time.deltaTime;
            
            if (_lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                _lobbyUpdateTimer = lobbyUpdateTimerMax;

               Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_myJoinedLobby.Id);
               _myJoinedLobby = lobby;
            }
        }
        
        
        
        
        #endregion

    }
    
    
    public struct PlayerLobbyData : INetworkSerializable, IEquatable<PlayerLobbyData>
    {
        public FixedString128Bytes PlayerName;
        public int PlayerCarIndex;
        public FixedString128Bytes PlayerId;
        public ulong ClientId;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref PlayerCarIndex);
            serializer.SerializeValue(ref PlayerId);
            serializer.SerializeValue(ref ClientId);

        }

        public bool Equals(PlayerLobbyData other)
        {
            return PlayerName == other.PlayerName && PlayerCarIndex == other.PlayerCarIndex && PlayerId == other.PlayerId && ClientId == other.ClientId;
        }
    }
}