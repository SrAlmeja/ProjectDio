using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class WaitingPlayersUI : MonoBehaviour
    {

        #region Serialized fields

        [SerializeField] private GameObject waitingPlayersUI;
        [SerializeField] private GameObject readyText;

        #endregion

        #region private Variables


        #endregion

        void Start()
        {
            DioGameManager.Instance.OnPlayerReady += HandleWaitingPlayersUI;
            DioGameManager.Instance.OnGameStateChange += HideUI;
                
        }
        
        #region private methods

        private void HandleWaitingPlayersUI(bool value)
        {
            if (value)
            {
                ShowImReady();
            }
        }

        #endregion

        #region public methods
        
        public void ShowImReady()
        {
            readyText.SetActive(true);
        }

        private void HideUI(DioGameManager.GameStates state)
        {
            if (DioGameManager.Instance.IsInCountDownState())
            {
                waitingPlayersUI.SetActive(false);
                
                DioGameManager.Instance.OnGameStateChange -= HideUI;
                DioGameManager.Instance.OnPlayerReady -= HandleWaitingPlayersUI;
            }
            
        }

        #endregion
    }
}
