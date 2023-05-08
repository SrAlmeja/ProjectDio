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

        void Update()
        {

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
            Debug.Log("HideUI called and get game state: " + DioGameManager.Instance.GetGameState());
            
            if (DioGameManager.Instance.IsInCountDownState())
            {
                waitingPlayersUI.SetActive(false);
            }
        }

        #endregion
    }
}
