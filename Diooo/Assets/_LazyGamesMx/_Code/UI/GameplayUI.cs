using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerTxt;
    
    private float _timer;
    
    
    private 
    void Start()
    {
        
    }

    void Update()
    {
        UpdateTimer();   
    }
    
    private void UpdateTimer()
    {
        if (DioGameManagerSingleplayer.Instance.MyGameState != DioGameManagerSingleplayer.GameStatesSingleplayer.GamePlaying) return;
            
        _timer = StopWatchManager.Instance.CurrentTime;
        timerTxt.text = _timer.ToString("F2");
        Debug.Log(_timer);
    }
    
}
