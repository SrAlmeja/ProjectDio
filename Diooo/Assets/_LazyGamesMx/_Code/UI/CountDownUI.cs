using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : MonoBehaviour
{
    [SerializeField] private GameObject countDownUI;
    [SerializeField] private Image imageCountDownUI;
    [SerializeField] private Sprite[] spritesCountDown;
    
    private int _previousSecond = 0;
    
    void Start()
    {
        countDownUI.SetActive(false);
        CountdownControllerMultiplayer.Instance.OnHandleCountdown += HandleCountdown;
        CountdownControllerMultiplayer.Instance.OnSecondPassed += HandleSecondPassed;
    }
    void OnDestroy()
    {
        CountdownControllerMultiplayer.Instance.OnHandleCountdown -= HandleCountdown;
        CountdownControllerMultiplayer.Instance.OnSecondPassed -= HandleSecondPassed;
    }
    
    void HandleSecondPassed(object sender, int seconds)
    {
        if (_previousSecond != seconds)
        {
            if(seconds >= 0 && seconds < spritesCountDown.Length)
                imageCountDownUI.sprite = spritesCountDown[seconds];
        }
        _previousSecond = seconds;
    }
    
    void HandleCountdown(object sender, bool isCountdownActive)
    {
        countDownUI.SetActive(isCountdownActive);
    }
    
}
