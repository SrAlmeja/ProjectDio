// Dino 11/04/2023 Creation of the script
//Control the scenes and the network scenes

using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.LazyGames.Dio
{
    public class SceneController : MonoBehaviour
    {
        #region public variables

        private static SceneController _instance;
        public static SceneController Instance
        {
            get
            {
                if (FindObjectOfType<SceneController>() == null)
                {
                    GameObject sceneControllerGO = new GameObject("SceneController");
                    sceneControllerGO.SetActive(false);
                    _instance = sceneControllerGO.AddComponent<SceneController>();
                    sceneControllerGO.SetActive(true);
                    DontDestroyOnLoad(sceneControllerGO);
                }

                return _instance;
            }
        }


        #endregion

        #region private variables

        private static string _targetScene;

        #endregion


        #region unity methods

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
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