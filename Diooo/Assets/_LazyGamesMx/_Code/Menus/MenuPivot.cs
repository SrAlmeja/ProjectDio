//Fernando Cossio 11/04/23
/// <summary>
/// This will be used as an adress directory implemented in each pivot, like a general street. 
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MenuPivot : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _pivotNumber;
        [SerializeField] private List<MenuAdress> _adressList;
        public int PivotNumber { get { return _pivotNumber; } }
        public List<MenuAdress> AdressList { get { return _adressList; } }
    }
}

