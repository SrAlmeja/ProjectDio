using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class WaitingMultiplayersUI : MonoBehaviour
    {

        #region Serialized fields

        [SerializeField] private GameObject waitingPlayersUI;
        [SerializeField] private GameObject readyText;

        #endregion

        #region private Variables


        #endregion

        void Start()
        {
            DioGameManagerMultiplayer.Instance.OnPlayerReady += HandleWaitingPlayersUI;
            DioGameManagerMultiplayer.Instance.OnGameStateChange += HideUI;
                
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

        private void HideUI(DioGameManagerMultiplayer.GameStates state)
        {
            if (DioGameManagerMultiplayer.Instance.IsInCountDownState())
            {
                waitingPlayersUI.SetActive(false);
                
                DioGameManagerMultiplayer.Instance.OnGameStateChange -= HideUI;
                DioGameManagerMultiplayer.Instance.OnPlayerReady -= HandleWaitingPlayersUI;
            }
            
        }

        #endregion
    }
}
