using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DioGameMultiplayer : MonoBehaviour
    {
        public static DioGameMultiplayer Instance;

        public event Action OnFinishLoading;
        private bool _isInitialized;

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
    }
}
