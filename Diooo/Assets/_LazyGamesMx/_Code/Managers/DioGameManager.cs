// Dino 05/04/2023 Creation of the script
// Gameplay controller of the game scene

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class DioGameManager : NetworkBehaviour
    {

        #region public variables

        public static DioGameManager Instance;

        #endregion

        #region private variables

        [SerializeField] private GameObject _playerCarPrefab;


        #endregion

        #region Unity Methods

        void Start()
        {

        }

        void Update()
        {

        }

        #endregion

        #region public methods

        public override void OnNetworkSpawn()
        {
            Instance = this;
            if (IsServer)
            {
                Debug.Log("Server is loading the scene");
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }
        }

        #endregion

        #region private methods

        private void SceneManager_OnLoadEventCompleted(string sceneName,
            UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted,
            List<ulong> clientsTimedOut)
        {
            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                GameObject playerTransform = Instantiate(_playerCarPrefab);
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
                Debug.Log("Spawned player for client: " + clientID);
            }
        }


        #endregion
    }
}