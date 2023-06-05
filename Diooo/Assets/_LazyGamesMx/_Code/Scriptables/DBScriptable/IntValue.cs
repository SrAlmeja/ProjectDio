using UnityEngine;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(fileName = "NewScriptableObject", menuName = "DBScriptables/IntValue")]

    public class IntValue : ScriptableObject
    {
        public int value;
    }
}