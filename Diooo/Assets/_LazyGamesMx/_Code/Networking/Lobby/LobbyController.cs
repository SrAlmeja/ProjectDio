//Dino 04/03/22  creaion of the class, here will control the matchmaking and user connection

using QFSW.QC;
using Unity.Netcode;
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
        
        
        #endregion


        #region Lobby

        [Command()]
        private async void CreateLobby()
        {
            try
            {
                string lobbyName = "DinoLobby";
                int maxPlayers = 4;

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
                Debug.Log("Created lobby with id: " + lobby.Name + " " + lobby.MaxPlayers);

            }
            catch (LobbyServiceException exception)
            {
                Debug.Log(exception);
            }
            
           


        }

        #endregion

    }
}