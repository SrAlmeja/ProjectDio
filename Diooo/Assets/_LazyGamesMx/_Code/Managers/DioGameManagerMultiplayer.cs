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
    public class DioGameManagerMultiplayer : NetworkBehaviour
    {

        #region public variables

        public static DioGameManagerMultiplayer Instance
        {
            get
            {
                if (FindObjectOfType<DioGameManagerMultiplayer>() == null)
                {
                    GameObject gameManagerGO = new GameObject("DioGameManager");
                    gameManagerGO.SetActive(false);
                    _instance = gameManagerGO.AddComponent<DioGameManagerMultiplayer>();

                    gameManagerGO.SetActive(true);
                    DontDestroyOnLoad(gameManagerGO);
                }

                return _instance;
            }
        }
        

        public Action<bool> OnPlayerReady;
        public Action<GameStates> OnGameStateChange;
        public Action<ClientRpcParams> OnFinishedSpawnPlayers;

        #endregion

        #region Serialized variables
        
        [Header("Player Spawn Points")]
        [SerializeField] private List<Transform> placesToSpawnCars;
        [Header("Player Prefab")]
        [SerializeField] private Transform playerCarPrefab;
        // [SerializeField] private Transform networkCameraPrefab;
        [Header("Events Player")]
        [SerializeField] private ReadyPlayerInput readyPlayerInput;
        
        [Header("Game State")] 
        [SerializeField]
        private NetworkVariable<GameStates> myGameState = new NetworkVariable<GameStates>(GameStates.WaitingToStart);
        
        #endregion

        #region private variables

        private static DioGameManagerMultiplayer _instance;
        
        private int _spawnIndex = 0;
        private int _playersConnected = 0;
        private bool _isLocalPlayerReady = false;
        
        private Dictionary<ulong,bool> playerReadyDictionary = new Dictionary<ulong, bool>();

        private GameStates MyGameState
        {
            get => myGameState.Value;
            set
            {
                myGameState.Value = value;
            }
        }

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
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
            
            //Game is in Singleplayer Mode
            if (!DioGameMultiplayer.Instance.IsHostInitialized.Value)
            {
                enabled = false;
            }
        }
        
        private void OnDisable()
        {
            readyPlayerInput.OnPlayerReadyInput -= OnGameInput_SetReady;
            myGameState.OnValueChanged -= (prevValue, newValue) =>
            { 
                 OnGameStateChange?.Invoke(newValue); 
            };
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
            
            MyGameState = GameStates.WaitingToStart;
            
            //Handle Ready Players Input
            readyPlayerInput.OnPlayerReadyInput += OnGameInput_SetReady;
            
            myGameState.OnValueChanged += (prevValue, newValue) =>
            {
                OnGameStateChange?.Invoke(newValue);
                Debug.Log("<color=#7DFF33>Game State changed to: </color>" + newValue);

            };
            
            //Handle Countdown
            CountdownControllerMultiplayer.Instance.OnCountdownFinished += OnCountdownFinished;

        }

        public bool IsInCountDownState()
        {
            if(MyGameState == GameStates.Countdown)
            {
                return true;
            }

            return false;
        }
        
        public GameStates GetGameState()
        {
            return MyGameState;
        }
        #endregion

        #region private methods

        private void HandleConnectedClients()
        {
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
                if (_spawnIndex > placesToSpawnCars.Count)
                {
                    Debug.Log("<color=#FE4A3B>Not enough spawn points</color>");
                    return;
                }
                
                Debug.Log("<color=#C9FE3B>Players spawned = </color>"+ _spawnIndex + " in object =  " + placesToSpawnCars[_spawnIndex - 1].name);
                
                Transform playerTransform = Instantiate(playerCarPrefab);
                playerTransform.name = "CAR CLIENT = "+ clientID;
                playerTransform.position = placesToSpawnCars[_spawnIndex - 1].position;
                
                NetworkObject networkCarObject = playerTransform.GetComponent<NetworkObject>();
                networkCarObject.SpawnAsPlayerObject(clientID, true);
                
                Debug.Log("<color=#7AEFFF>Spawned player for clientID: </color>" + clientID ); 
            }
        }
        
        
        private void OnGameInput_SetReady()
        {
            if (MyGameState == GameStates.WaitingToStart)
            {
                _isLocalPlayerReady = true;
                SetPlayerServerRPC();
                OnPlayerReady?.Invoke(_isLocalPlayerReady);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerServerRPC(ServerRpcParams serverRpcParams = default)
        {
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
            if (allClientsReady)
            {
                MyGameState = GameStates.Countdown;
            }
        }
        
        private void OnCountdownFinished()
        {
            if (!IsServer) return;
            MyGameState = GameStates.GamePlaying;
        }
        
        #endregion
    }
    
}