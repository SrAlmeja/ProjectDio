using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.CloudSave;

namespace com.LazyGames.Dio
{ 
    public class CloudSaveController : MonoBehaviour
    {
        public static CloudSaveController Instance;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }

        public async void SendTestCloudSave()
        {
            var data = new Dictionary<string, object>{ { "MySaveKey", "HelloWorld" } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data); 
            Debug.Log("<color=#C4FF92>Cloud save test  </color>");
        }
        
        public async void SendCloudSave(string key, string value)
        {
            var data = new Dictionary<string, object>{ { key, value } };
            await CloudSaveService.Instance.Data.ForceSaveAsync(data); 
            Debug.Log("<color=#C4FF92>Cloud save test  </color>");
        }
    }
}
