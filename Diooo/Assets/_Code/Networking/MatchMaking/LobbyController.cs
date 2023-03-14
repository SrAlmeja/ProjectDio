//Dino 04/03/22  creaion of the class, here will control the matchmaking and user connection
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
        
        void Start()
        {
            serverButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartServer();
                Debug.Log("Server started");
            });
            clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
                Debug.Log("Client started");
            });
            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
                Debug.Log("Host started");
            });
        }

        void Update()
        {

        }
        
        #endregion

    }
}