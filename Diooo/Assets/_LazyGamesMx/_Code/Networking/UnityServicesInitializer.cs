using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;

namespace com.LazyGames.Dio
{
    public class UnityServicesInitializer : MonoBehaviour
    {
        public static UnityServicesInitializer Instance;

        public Action OnFinishedInitUnityServices;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // InitUnityServices();
        }
        
        
        
        
        
        private async void InitUnityServices() 
        { 
            await UnityServices.InitializeAsync();
            Debug.Log("<color=#C4FF92>Unity Services Initialized? = </color>");
            OnFinishedInitUnityServices?.Invoke();
        }
    }
}
