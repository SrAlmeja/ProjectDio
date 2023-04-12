//Dino this script manage the multiplayer of the game
//Decides who is the host and who is the client
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DioGameMultiplayer : MonoBehaviour
    {
        #region public variables
        public static DioGameMultiplayer Instance;
        public event Action OnFinishLoading;
        #endregion
        
        #region private variables
        private bool _isInitialized;
        #endregion
        
        #region Unity Methods
        private void Awake()
        {
            Instance = this;
            _isInitialized = true;
            if (_isInitialized)
            {
                OnFinishLoading?.Invoke();
            }
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
        }
        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Start Client");
        }
        #endregion

    }
}
