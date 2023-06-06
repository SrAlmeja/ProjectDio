using UnityEngine;
using TMPro;

namespace com.LazyGames.Dio
{
    public class ShowHighScores : MonoBehaviour
    {
        public DatabaseManager databaseManager;
        public TMP_Text Name;
        public TMP_Text Race;
        public TMP_Text Time;

        private void OnEnable()
        {
            Name.text = databaseManager.GetTop10Names();
            Race.text = databaseManager.GetTop10Race();
            Time.text = databaseManager.GetTop10Time();
        }
    }
}