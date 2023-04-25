using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

public class WaitingPlayersUI : MonoBehaviour
{

    #region Serialized fields

    [SerializeField] private GameObject waitingPlayersUI;
    
    #endregion
    #region private Variables
    

    #endregion
    void Start()
    {
        
        ShowWaitingPlayersUI();
        DioGameManager.Instance.OnPlayerReady += HandleWaitingPlayersUI;
        
    }

    void Update()
    {
        
    }

    #region private methods

    private void HandleWaitingPlayersUI(bool value)
    {
        if (value)
        {
            HideWaitingPlayersUI();
        }
    }
    #endregion
    
    #region public methods
    
    public void ShowWaitingPlayersUI()
    {
        waitingPlayersUI.SetActive(true);
    }
    
    public void HideWaitingPlayersUI()
    {
        waitingPlayersUI.SetActive(false);
    }
    
    #endregion
}
