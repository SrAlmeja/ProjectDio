// Dino 11/04/2023 Creation of the script
//Control the scenes and the network scenes

using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.LazyGames.Dio
{
    public class SceneController : MonoBehaviour
    {
        #region public variables

        public static SceneController Instance;



        #endregion

        #region private variables

        private static string _targetScene;

        #endregion


        #region unity methods

        void Start()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        void Update()
        {

        }

        #endregion

        #region public methods



        public void LoadScene(string sceneKey)
        {
            SceneManager.LoadSceneAsync(sceneKey);
        }

        public void LoadSceneNetwork(string sceneKey)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneKey, LoadSceneMode.Single);
        }

        #endregion

    }
}