//Dino this script manage the multiplayer of the game
//Decides who is the host and who is the client
using System;
using System.Collections;
using System.Collections.Generic;
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
        private NetworkVariable<bool> _isHostInitialized = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public NetworkVariable <bool> IsHostInitialized
        {
            get => _isHostInitialized;
        }
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
            
        }
        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
            OnStartClient?.Invoke();
        }
        #endregion
        
        
    }

    public struct PlayerData : INetworkSerializable
    {
        public string PlayerName;
        public int PlayerImageIndex;
        public string PlayerId;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref PlayerName);
            serializer.SerializeValue(ref PlayerImageIndex);
            serializer.SerializeValue(ref PlayerId);
        }
    }
    
    
    
}



