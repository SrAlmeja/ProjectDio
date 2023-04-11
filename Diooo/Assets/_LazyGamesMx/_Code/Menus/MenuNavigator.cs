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
        [Header("Initial Dependency")]
        [SerializeField] private MenuAdress _currentAdress;

        private void ChangeAdress(MenuAdress newAdress)
        {
            _currentAdress = newAdress;
        }

        private void CheckDeadEnd(MenuAdress adress)
        {
           
        }
    }
}
