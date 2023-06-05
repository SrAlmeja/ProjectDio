using UnityEngine;
using TMPro;

namespace com.LazyGames.Dio
{
    public class ShowHighScores : MonoBehaviour
    {
        public DatabaseManager databaseManager;
        public TMP_Text textMeshPro;

        private void OnEnable()
        {
            textMeshPro.text = databaseManager.GetTop10();
        }
    }
}