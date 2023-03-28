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

        private void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
