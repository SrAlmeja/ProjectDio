using UnityEngine;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(fileName = "NamePlayer_SO", menuName = "ScriptableObjects/NamePlayer_SO")]

    public class NameToDB_SO : ScriptableObject
    {

        public string _name;
        public int _id;
       
        public void SetNameDB(string name)
        {
            _name = name;
            DioGameManagerSingleplayer.Instance.databaseManager.InsertName(GetName());
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

    }

}
