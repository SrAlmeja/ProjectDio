//Fernando Cossio 11/04/23
/// <summary>
/// This will track in what pivot and number we are currently. It will aso request animations to navigate menus. 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MenuNavigator : MonoBehaviour
    {
        [Header("SO Channel Dependencies")]
        [SerializeField] private IntEventChannelSO _navigationRequestChannel;

        [Header("Initial Dependency")]
        [SerializeField] private MenuAdress _currentAdress;

        private void ChangeAdress(MenuAdress newAdress)
        {
            _currentAdress = newAdress;
        }

        private bool CheckDeadEndForward(MenuAdress adress)
        {
            return adress.ForwardTo == null;
        }

        private bool CheckDeadEndBackward(MenuAdress adress)
        {
            return adress.BackTo == null;
        }

        private void MoveForward()
        {
            if (CheckDeadEndForward(_currentAdress))
            {
                _currentAdress.DoForwardAction();
                return;
            }

            RequestNavigation(_currentAdress.ForwardTo);
        }

        private void MoveBackward()
        {
            if (CheckDeadEndBackward(_currentAdress))
            {
                _currentAdress.DoBackAction();
                return;
            }

            RequestNavigation(_currentAdress.BackTo);
        }

        private void RequestNavigation(MenuAdress toAdress)
        {
            //TODO once received the new adress we want to go.

            _navigationRequestChannel.DoubleIntEvent(toAdress.Pivot, toAdress.Number);
        }
    }
}
