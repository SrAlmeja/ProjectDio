using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.LazyGames.Dio
{
    public class NameDBController : MonoBehaviour
    {
        [SerializeField] private NameToDB_SO nameToDbSo;
        [SerializeField] private GameObject nameDBPrefab;
        [SerializeField] private TMP_InputField inputField;
        
        private string playerName;
        
        
        void Start()
        {
        }

        public void GetNameFromInput()
        {
            playerName = inputField.text;
            nameToDbSo.SetNameDB(playerName);
            nameDBPrefab.SetActive(false);
        }
        
        
        
        
    }
}