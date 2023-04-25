// Dino 05/04/2023 Creation of the script
// Gameplay controller of the game scene

using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class DioGameManager : NetworkBehaviour
    {
        
        #region public variables

        public static DioGameManager Instance
        {
            get
            {
                if (FindObjectOfType<DioGameManager>() == null)
                {
                    GameObject gameManagerGO = new GameObject("DioGameManager");
                    gameManagerGO.SetActive(false);
                    _instance = gameManagerGO.AddComponent<DioGameManager>();

                    gameManagerGO.SetActive(true);
                    DontDestroyOnLoad(gameManagerGO);
                }

                return _instance;
            }
        }

        public int PlayersConnected
        {
            get => _playersConnected;
            set => _playersConnected = value;
        }
        public GameStates MyGameState
        {
            get => _myGameState;
            set => _myGameState = value;
        }

        public bool IsLocalPlayerReady()
        {
            return _isLocalPlayerReady;
        }
        public Action<bool> OnPlayerReady;

        #endregion

        #region Serialized variables
        
        [Header("Player Spawn Points")]
        [SerializeField] private List<Transform> placesToSpawnCars;
        [Header("Player Prefab")]
        [SerializeField] private Transform playerCarPrefab;
        [Header("Countdown Settings")]
        [SerializeField] private int countdownTime = 3;
        
        #endregion

        #region private variables

        private static DioGameManager _instance;
        private GameStates _myGameState = GameStates.None;
        
        private int _spawnIndex = 0;
        private int _playersConnected = 0;
        private bool _isLocalPlayerReady = false;
        

        public enum GameStates
        {
            None,
            WaitingToStart,
            Countdown,
            GamePlaying,
            GameOver
        }


        #endregion

        #region Unity Methods

        private void Awake()
        {
            _instance = this;
        }

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
            _instance = this;
            if (IsServer)
            {
                // Debug.Log("Server is loading the scene");
                Debug.Log("<color=#3B97FE>Number of players connected </color>" + NetworkManager.Singleton.ConnectedClientsIds.Count);
                _playersConnected = NetworkManager.Singleton.ConnectedClientsIds.Count;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }
            
            _myGameState = GameStates.WaitingToStart;
        }
        #endregion

        #region private methods

        private void SceneManager_OnLoadEventCompleted(string sceneName,
            UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted,
            List<ulong> clientsTimedOut)
        {
            foreach (var clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                _spawnIndex++;
                if (_spawnIndex >= placesToSpawnCars.Count)
                {
                    Debug.Log("<color=#FE4A3B>Not enough spawn points</color>");
                    return;
                }
                
                // Debug.Log("<color=#C9FE3B>Spawned index players = </color>"+ _spawnIndex + " in object =  " + placesToSpawnCars[_spawnIndex].name);
                
                Transform playerTransform = Instantiate(playerCarPrefab);
                playerTransform.position = placesToSpawnCars[_spawnIndex].position;
                NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();
                networkObject.SpawnAsPlayerObject(clientID, true);
                Debug.Log("<color=#7AEFFF>Spawned player for client: </color>" + clientID + gameObject.name); 
            }
        }

        
        private void OnGameInput_ReadyInteraction()
        {
            if (MyGameState == GameStates.WaitingToStart)
            {
                _isLocalPlayerReady = true;
                OnPlayerReady?.Invoke(_isLocalPlayerReady);
            }
        }
        
        #endregion
    }
}