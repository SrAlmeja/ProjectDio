using System;
using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

public class CountDownController_Singleplayer : MonoBehaviour
{
    [SerializeField] private float countdownTime = 3f;
    private bool _isTimerActive = false;
    private float _countdownTimer = 0f;
    public static CountDownController_Singleplayer Instance;
    public EventHandler<int> OnSecondPassed;
    public EventHandler<bool> OnHandleCountdown;
    public event Action OnCountdownFinished;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _isTimerActive = false;
        _countdownTimer = countdownTime;
        DioGameManagerSingleplayer.Instance.OnGameStateChange += HandleGameStateChange;
    }

    void Update()
    {
        if (_isTimerActive)
        {
            if (_countdownTimer <= 0)
            {
                StopCountdown();
                return;
            }
            _countdownTimer -= Time.deltaTime;
            OnSecondPassed?.Invoke(this, (int)_countdownTimer);
        }
    }

    private void StartCountdown()
    {
        _isTimerActive = true;
        OnHandleCountdown?.Invoke(this, _isTimerActive);
    }

    private void StopCountdown()
    {
        _isTimerActive = false;
        OnHandleCountdown?.Invoke(this, _isTimerActive);
        OnCountdownFinished?.Invoke();
    }
    private void HandleGameStateChange(DioGameManagerSingleplayer.GameStatesSingleplayer state)
    {
        if (state == DioGameManagerSingleplayer.GameStatesSingleplayer.Countdown)
        {
            StartCountdown();
        }
            
    }
}
