using System;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;



namespace com.LazyGames.Dio
{ 
    public class AuthenticatorController : MonoBehaviour
    {
        
        #region public variables

        public static AuthenticatorController Instance
        {
            get
            {
                if (FindObjectOfType<AuthenticatorController>() == null)
                {
                    GameObject authenticatorGO = new GameObject("AuthenticatorController");
                    authenticatorGO.SetActive(false);
                    _instance = authenticatorGO.AddComponent<AuthenticatorController>();
                    authenticatorGO.SetActive(true);
                    // DontDestroyOnLoad(authenticatorGO);
                    
                }

                return _instance;
            }
        }
        

        public Action OnFinishedAuthenticating;
        public Action OnFinishedAnonymousLogin;

        #endregion

        #region private variables

        private static AuthenticatorController _instance;

        #endregion
        #region Unity methods

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            if (!ConnectionNetworking.Instance.HasInternet)
                return;

            DoAnonymousLogin();
        }

        void Update()
        {
            
        }
        #endregion

        #region public methods

        public async void DoAnonymousLogin()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                InitializationOptions initializationOptions = new InitializationOptions();
                initializationOptions.SetProfile(UnityEngine.Random.Range(0, 1000).ToString());

                await UnityServices.InitializeAsync(initializationOptions);
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                
                Debug.Log("<color=#C4FF92>Signed in PLAYER ID = </color>" + AuthenticationService.Instance.PlayerId);
                OnFinishedAnonymousLogin?.Invoke();
                CloudSaveController.Instance.SendTestCloudSave();

            }
        }

        #endregion
        
    }

}
