//Dino 03/03/23 Creation of the script
//This script is used to check if th player has internet connection

using UnityEngine;
namespace com.LazyGames.Dio
{
    public class ConnectionNetworking : MonoBehaviour
    {
        #region public variables 
        
        public static ConnectionNetworking Instance;

        public bool HasInternet => _hasInternet;
        
        [SerializeField] private GameObject internetConnectionPanel;
        #endregion
        
        #region private variables
        private bool _hasInternet;
        private string _rechabilityTxt;
        
        
        #endregion

        #region Unity Methods

        void Start()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            // InitializerNetworking.Instance.OnFinishLoading += Initialize;
            Initialize();
        }

        #endregion

        #region public methods

        #endregion
        
        #region private methods

        void Initialize()
        {
            Debug.Log("ConnectionNetworking Initialized");
            CheckConnection();
        }

        void CheckConnection()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    _hasInternet = false;
                    _rechabilityTxt = "No internet connection";
                    internetConnectionPanel.SetActive(true);
                    break;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    _hasInternet = true;
                    _rechabilityTxt = "Internet connection via carrier data network";
                    internetConnectionPanel.SetActive(false);
                    break;
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    _hasInternet = true;
                    _rechabilityTxt = "Internet connection via local area network";
                    internetConnectionPanel.SetActive(false);
                    break;
            }
            
            Debug.Log(_rechabilityTxt);
        }
        #endregion
    }
}