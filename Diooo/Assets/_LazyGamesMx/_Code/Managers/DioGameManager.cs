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
        // public GameStates MyGameState
        // {
        //     get => myGameState;
        //     set => myGameState = value;
        // }
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
        [SerializeField] private float countdownTimer = 3;
        
        [FormerlySerializedAs("startGameInput")]
        [Header("Events Player")]
        [SerializeField] private ReadyPlayerInput readyPlayerInput;
        [SerializeField] private EnableInputsPlayer enableInputsPlayer;
        
        [Header("Game State")] 
        [SerializeField]
        private NetworkVariable<GameStates> myGameState = new NetworkVariable<GameStates>(GameStates.WaitingToStart);

        
        
        #endregion

        #region private variables

        private static DioGameManager _instance;
        
        private int _spawnIndex = 0;
        private int _playersConnected = 0;
        private bool _isLocalPlayerReady = false;
        
        private Dictionary<ulong,bool> playerReadyDictionary = new Dictionary<ulong, bool>();


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
            if (!IsServer)
            {
                return;
            }

            switch (myGameState.Value)
            {
                case GameStates.WaitingToStart:
                    if (NetworkManager.Singleton.ConnectedClientsIds.Count == 2)
                    {
                        myGameState.Value = GameStates.Countdown;
                    }
                    break;
            }
            
        }

        #endregion

        #region public methods

        public override void OnNetworkSpawn()
        {
            _instance = this;
            
            if (IsServer)
            {
               HandleConnectedClients();
            }
            
            Debug.Log("<color=#7DFF33>Set game state to waiting to start</color>");
            myGameState.Value = GameStates.WaitingToStart;
            
            //Handle Ready Players Input
            readyPlayerInput.OnPlayerReadyInput += OnGameInput_SetReady;

        }
        #endregion

        #region private methods

        private void HandleConnectedClients()
        {
            // Debug.Log("Server is loading the scene");
            Debug.Log("<color=#3B97FE>Number of players connected </color>" + NetworkManager.Singleton.ConnectedClientsIds.Count);
            _playersConnected = NetworkManager.Singleton.ConnectedClientsIds.Count;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
        
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
                Debug.Log("<color=#7AEFFF>Spawned player for clientID: </color>" + clientID ); 
            }
        }

        
        private void OnGameInput_SetReady()
        {
            if (myGameState.Value == GameStates.WaitingToStart)
            {
                _isLocalPlayerReady = true;
                SetPlayerServerRPC();
                OnPlayerReady?.Invoke(_isLocalPlayerReady);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerServerRPC(ServerRpcParams serverRpcParams = default)
        {
            
            Debug.Log(serverRpcParams.Receive.SenderClientId + "<color=#FF70E3> is ready</color>");
            playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
            
            bool allClientsReady = true;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
                {
                    allClientsReady = false;
                    break;
                }
            }
            Debug.Log("<color=#C2FF70>all clients ready = </color>" + allClientsReady);
        }


        private void HandleTimerCountdown()
        {
            countdownTimer -= 1 * Time.deltaTime;
        }
        
        #endregion
    }
    
}