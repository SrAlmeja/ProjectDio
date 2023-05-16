using System;
using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : NetworkBehaviour
{
    [SerializeField] private float countdownTime = 3f;

    private bool _isTimerActive = false;
    private NetworkVariable<float> _countdownTimer = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    public static CountdownController Instance;

    public EventHandler<int> OnSecondPassed;
    public EventHandler<bool> OnHandleCountdown;
    
    public Action OnCountdownFinished;

    private void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        Instance = this;
        _isTimerActive = false;
        _countdownTimer.Value = countdownTime;
        DioGameManager.Instance.OnGameStateChange += HandleGameStateChange;
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        
        if (_isTimerActive)
        {
            if (_countdownTimer.Value <= 0)
            {
                StopCountdown();
                StopCountdownClientRpc();
                return;
            }
            
            _countdownTimer.Value -= Time.deltaTime;
            OnSecondPassed?.Invoke(this, (int)_countdownTimer.Value);
            SendSecondsToClientRpc();
        }
        
    }

    [ClientRpc]
    private void SendSecondsToClientRpc()
    {
        OnSecondPassed?.Invoke(this, (int)_countdownTimer.Value);
    }
    [ClientRpc]
    private void StopCountdownClientRpc()
    {
        StopCountdown();
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

    private void HandleGameStateChange(DioGameManager.GameStates state)
    {
        if (state == DioGameManager.GameStates.Countdown)
        {
            StartCountdown();
        }
    }
    
}
