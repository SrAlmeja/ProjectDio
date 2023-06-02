using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingSIngleplayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _waitingPanels;
    
    void Start()
    {
        
        DioGameManagerSingleplayer.Instance.OnGameStateChange += HideUI;
    }
    
    
    
    private void HideUI(DioGameManagerSingleplayer.GameStatesSingleplayer state)
    {
        if (DioGameManagerSingleplayer.Instance.MyGameState == DioGameManagerSingleplayer.GameStatesSingleplayer.Countdown)
        {
            _waitingPanels.SetActive(false);
            DioGameManagerSingleplayer.Instance.OnGameStateChange -= HideUI;
        }
    }
}
