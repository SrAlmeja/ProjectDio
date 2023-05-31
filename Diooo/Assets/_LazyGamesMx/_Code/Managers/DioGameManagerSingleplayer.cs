// Dino 19/05/23 Creation of variant of the script for singleplayer

using System;
using com.LazyGames.Dio;
using UnityEngine;
using UnityEngine.Serialization;

public class DioGameManagerSingleplayer : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private VoidEventChannelSO OnSinglePlayerReady;
    [SerializeField] private SinglePlayerGoal singlePlayerGoal;
    #endregion    
    
    #region private variables
    
    private static DioGameManagerSingleplayer _instance;

    private GameStatesSingleplayer _gameStatesSingleplayer;
    public GameStatesSingleplayer  MyGameState
    {
        get => _gameStatesSingleplayer;
        set => _gameStatesSingleplayer = value;
    }

    #endregion

    #region public variables
    
    public static DioGameManagerSingleplayer Instance
    {
        get
        {
            if (FindObjectOfType<DioGameManagerSingleplayer>() == null)
            {
                GameObject gameManagerGO = new GameObject("DioGameManager");
                gameManagerGO.SetActive(false);
                _instance = gameManagerGO.AddComponent<DioGameManagerSingleplayer>();
                gameManagerGO.SetActive(true);
                DontDestroyOnLoad(gameManagerGO);
            }

            return _instance;
        }
    }
    
    public enum GameStatesSingleplayer
    {
        None,
        WaitingToCinematic,
        Countdown,
        GamePlaying,
        GameOver
    }
    
    public Action<GameStatesSingleplayer> OnGameStateChange;
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

    void Start()
    {
        MyGameState = GameStatesSingleplayer.WaitingToCinematic;
        OnSinglePlayerReady.VoidEvent += HandleOnFinishedCinematic;
        singlePlayerGoal.OnPlayerCrossedGoal += HandleOnFinishedCinematic;
    }

    void Update()
    {
        
    }
    #endregion

    #region public Methods
    
    
    #endregion

    #region private Methods

    private void HandleOnFinishedCinematic()
    {
        MyGameState = GameStatesSingleplayer.Countdown;
        OnGameStateChange?.Invoke(MyGameState);
    }

    #endregion
}
