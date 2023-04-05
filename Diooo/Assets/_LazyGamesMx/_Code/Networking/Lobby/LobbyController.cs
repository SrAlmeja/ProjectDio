//Dino 15/03/22  Lobby Connection

using System;
using System.Collections.Generic;
using Mono.CSharp;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;


namespace com.LazyGames.Dio
{
    public class LobbyController : MonoBehaviour
    {

        #region public variables

        public static LobbyController Instance;
        
       [SerializeField] string lobbyName = "DinoLobby";
       [SerializeField] int maxPlayers = 4;

       public Action OnPlayerEnterRoom;
       public Action OnCreatedLobbyStarted;

        #endregion
        
        #region private variables

        // private Lobby _hostLobby;
        private Lobby _myJoinedLobby;
        private float _heartbeatTimer = 0.0f;
        private float _lobbyUpdateTimer = 0.0f;
        
        #endregion

        #region unity methods
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!ConnectionNetworking.Instance.HasInternet)
                return;

            InitializeUnityAuthentication();
            CreateLobby();
            // ListLobbies();

        }
        
        private void Update()
        {
            HandleLobbyHeartbeat();
            HandleLobbyPollUpdate();
        }
        #endregion


        #region Unity Servicies

        //Autehntication Anonmoous
        private async void InitializeUnityAuthentication()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                InitializationOptions initializationOptions = new InitializationOptions();
                initializationOptions.SetProfile(UnityEngine.Random.Range(0, 1000).ToString());
                
                await UnityServices.InitializeAsync(initializationOptions);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            }
            
            // AuthenticationService.Instance.SignedIn += () =>
            // {
            //     Debug.Log("Signed in PLAYER ID = " + AuthenticationService.Instance.PlayerId );
            // };
        }
        

        #endregion
        
        
        
        #region Lobby

        private async void CreateLobby()
        {
            try
            {
                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                };
                
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
                _myJoinedLobby = lobby;
                
                Debug.Log("Created lobby with id: " + lobby.Name + " " + lobby.MaxPlayers + " LOBBY ID =  " + lobby.Id);
                
                DioGameMultiplayer.Instance.StartHost();
                OnCreatedLobbyStarted?.Invoke();

            }
            catch (LobbyServiceException exception)
            {
                Debug.Log(exception);
            }
        }

        private async void ListLobbies()
        {
            try
            {
                QueryResponse queryResponse =  await Lobbies.Instance.QueryLobbiesAsync();
                Debug.Log("Lobbies found: " + queryResponse.Results.Count);
                foreach (Lobby lobby in queryResponse.Results)
                {
                    Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
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
                await LobbyService.Instance.QuickJoinLobbyAsync();
                DioGameMultiplayer.Instance.StartClient();
                OnPlayerEnterRoom?.Invoke();
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
        // private Player GetPlayer()
        // {
        //     return new Player
        //     {
        //         // Id = AuthenticationService.Instance.PlayerId,
        //         Data = new Dictionary<string, PlayerDataObject>
        //         {
        //             {"Player Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
        //         }
        //     };
        // }
        // private void PrintPlayers(Lobby lobby)
        // {
        //     Debug.Log("Players in lobby: " + lobby.Players.Count); 
        //     foreach (Player player in lobby.Players)
        //     {
        //         Debug.Log("MY PLAYER ID =" +player.Id + " MY PLAYER NAME " + player.Data["Player Name"].Value);  
        //     }
        // }

        //Upadate player Data
        // private async void UpdatePlayerName(string newPLayerName)
        // {
        //     try
        //     {
        //         playerName = newPLayerName;
        //         await LobbyService.Instance.UpdatePlayerAsync(_myJoinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
        //         {
        //             Data = new Dictionary<string, PlayerDataObject>
        //             {
        //                 {"Player Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, newPLayerName)}
        //             }
        //         });
        //
        //     }
        //     catch (LobbyServiceException e)
        //     {
        //         Debug.Log(e);
        //         throw;
        //     }
        //     
        // }
        //
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
}