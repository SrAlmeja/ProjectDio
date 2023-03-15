//Dino 15/03/22  Lobby Connection

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
                Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId );
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
                JoinLobby();
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

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
                _hostLobby = lobby;
                
                Debug.Log("Created lobby with id: " + lobby.Name + " " + lobby.MaxPlayers);

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

        private async void JoinLobby()
        {
            try
            {
                QueryResponse queryResponse =  await Lobbies.Instance.QueryLobbiesAsync();
                await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
                
                Debug.Log("Joined lobby with id: " + queryResponse.Results[0].Id);
                
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

        }
        
        #endregion

       
    }
}