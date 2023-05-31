//Dino this script manage the multiplayer of the game
//Decides who is the host and who is the client
using System;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DioGameMultiplayer : NetworkBehaviour
    {
        #region public variables
        public static DioGameMultiplayer Instance;
        public  Action OnStartHost;
        public  Action OnStartClient;
        #endregion
        
        #region private variables
        private NetworkVariable<bool> _isHostInitialized = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Server);

        public NetworkVariable <bool> IsHostInitialized
        {
            get => _isHostInitialized;
        }
        public bool IsHost
        {
            get => _isHost;
        }
        private bool _isHost;
        
        public bool IsClient
        {
            get => _isClient;
        }
        private bool _isClient;
        #endregion
        
        #region Unity Methods
        private void Awake()
        {
            Instance = this;
            // _isInitialized = true;
            // // if (_isInitialized)
            // {
            //     OnFinishLoading?.Invoke();
            // }
            DontDestroyOnLoad(gameObject);

        }

        private void Start()
        {
        }

        #endregion

        #region public methods

        public void StartHost()
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Start Host");
            OnStartHost?.Invoke();
            _isHostInitialized.Value = true;
            _isHost = true;
            
        }
        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
            OnStartClient?.Invoke();
            _isClient = true;
        }
        #endregion
        
        
    }

 
    
    
    
}



