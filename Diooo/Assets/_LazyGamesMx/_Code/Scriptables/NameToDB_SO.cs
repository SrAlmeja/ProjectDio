using UnityEngine;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(fileName = "NamePlayer_SO", menuName = "ScriptableObjects/NamePlayer_SO")]

    public class NameToDB_SO : ScriptableObject
    {

        private NameDB nameDB;
        public NameDB NameDB { get => nameDB; set => nameDB = value; }

        public void SetNameDB(string name)
        {
            nameDB.name = name;
            DioGameManagerSingleplayer.Instance.databaseManager.InsertName(GetName());
        }

        public string GetName()
        {
            return nameDB.name;
        }

        public int GetId()
        {
            return nameDB.id;
        }

    }

    [System.Serializable]
    public class NameDB
    {
        public string name;
        public int id;
    }
}
