// Dino 05/04/2023 Creation of the script
// Gameplay controller of the game scene

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class DioGameManager : NetworkBehaviour
    {

        #region public variables

        public static DioGameManager Instance;

        #endregion

        #region private variables

        [SerializeField] private List<Transform> placesToSpawnCars;
        [SerializeField] private Transform playerCarPrefab;


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
                // Debug.Log("Server is loading the scene");
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }
        }

        #endregion

        #region private methods

        private void SceneManager_OnLoadEventCompleted(string sceneName,
            UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted,
            List<ulong> clientsTimedOut)
        {
            
            Debug.Log("<color=#C9FE3B>Number of players connected </color>" + NetworkManager.Singleton.ConnectedClientsIds.Count);
            
            int spawnIndex = 0;
            foreach (var clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                spawnIndex++;
                if (spawnIndex >= placesToSpawnCars.Count)
                {
                    Debug.Log("<color=#FE4A3B>Not enough spawn points</color>");
                    return;
                }
                
                Debug.Log("<color=#C9FE3B>Spawned index players = </color>"+ spawnIndex);
                
                Transform playerTransform = Instantiate(playerCarPrefab);
                playerTransform.position = placesToSpawnCars[spawnIndex].position;
                NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();
                networkObject.SpawnAsPlayerObject(clientID, true);
                Debug.Log("<color=#7AEFFF>Spawned player for client: </color>" + clientID + gameObject.name); 
            }
        }


        #endregion
    }
}