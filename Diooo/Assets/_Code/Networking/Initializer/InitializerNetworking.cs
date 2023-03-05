//Dino 04/03/22 This will be the initializer if we need to put events or methods before other scripts but i don't know ehy it is not working
using System;
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
