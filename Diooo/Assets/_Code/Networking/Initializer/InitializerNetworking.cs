using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{ 
    public class InitializerNetworking : MonoBehaviour
    {
        #region public variables
        
        public static InitializerNetworking Instance;
        public event Action OnFinishLoading;

        #endregion

        private bool _isInitialized;
        #region unity methods

        private void Awake()
        {
            Instance = this;
            _isInitialized = true;
            if (_isInitialized)
            {
                OnFinishLoading?.Invoke();
            }
        }

        void Start()
        {
          

        }

        void Update()
        {

        }
        #endregion

    }
}
