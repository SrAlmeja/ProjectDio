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
        CountdownController.Instance.OnHandleCountdown += HandleCountdown;
        CountdownController.Instance.OnSecondPassed += HandleSecondPassed;
    }
    
    void HandleSecondPassed(object sender, int seconds)
    {
        if (_previousSecond != seconds)
        {
            Debug.Log(seconds);
            imageCountDownUI.sprite = spritesCountDown[seconds];

        }
        _previousSecond = seconds;
    }
    
    void HandleCountdown(object sender, bool isCountdownActive)
    {
        countDownUI.SetActive(isCountdownActive);
    }
    
}
