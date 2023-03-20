//Dino 15/03/22  Lobby Connection

using System;
using System.Collections.Generic;
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

        public string codeLobby;
        public string playerName;

        #endregion
        
        #region private variables
        [SerializeField] private Button serverButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button hostButton;

        
        private Lobby _hostLobby;
        private float _heartbeatTimer = 0.0f;
        
        #endregion

        #region unity methods
        
        // void Start()
        // {
        //     //Test the connection UI
        //     serverButton.onClick.AddListener(() =>
        //     {
        //         NetworkManager.Singleton.StartServer();
        //         Debug.Log("Server started");
        //     });
        //     clientButton.onClick.AddListener(() =>
        //     {
        //         NetworkManager.Singleton.StartClient();
        //         Debug.Log("Client started");
        //     });
        //     hostButton.onClick.AddListener(() =>
        //     {
        //         NetworkManager.Singleton.StartHost();
        //         Debug.Log("Host started");
        //     });
        // }

        private async void Start()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in PLAYER ID = " + AuthenticationService.Instance.PlayerId );
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateLobby();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                ListLobbies();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                // QuickJoinLobby();
                JoinLobbyByCode(codeLobby);
            }
            
            HandleLobbyHeartbeat();
        }
        #endregion


        #region Lobby

        private async void CreateLobby()
        {
            try
            {
                string lobbyName = "DinoLobby";
                int maxPlayers = 4;

                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = GetPlayer()
                };
                
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
                _hostLobby = lobby;
                
                Debug.Log("Created lobby with id: " + lobby.Name + " " + lobby.MaxPlayers + " LOBBY ID =  " + lobby.Id);
                
                PrintPlayers(_hostLobby);

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
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Count = 25,
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                    },
                    Order = new List<QueryOrder>
                    {
                        new QueryOrder(false, QueryOrder.FieldOptions.Created)
                    }
                };
                
                
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

        

        private async void JoinLobbyByCode(string lobbyCode)
        {
            try
            {
                // QueryResponse queryResponse =  await Lobbies.Instance.QueryLobbiesAsync();
                
                JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
                {
                    Player = GetPlayer()
                };
                
               Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
                
                Debug.Log("Joined lobby with code: " + lobbyCode);
                
                PrintPlayers(joinedLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

        }

        private async void QuickJoinLobby()
        {
            try
            { 
                await LobbyService.Instance.QuickJoinLobbyAsync();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                throw;
            }
        }

        private Player GetPlayer()
        {
            return new Player
            {
                // Id = AuthenticationService.Instance.PlayerId,
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"Player Name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                }
            };
        }
        private void PrintPlayers(Lobby lobby)
        {
            Debug.Log("Players in lobby: " + lobby.Players.Count); 
            foreach (Player player in lobby.Players)
            {
                Debug.Log("MY PLAYER ID =" +player.Id + " MY PLAYER NAME " + player.Data["Player Name"].Value);  
            }
        }
        
        private async void HandleLobbyHeartbeat()
        {
            if(_hostLobby != null)
                _heartbeatTimer -= Time.deltaTime;
            if (_heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15f;
                _heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
        #endregion

       
    }
}