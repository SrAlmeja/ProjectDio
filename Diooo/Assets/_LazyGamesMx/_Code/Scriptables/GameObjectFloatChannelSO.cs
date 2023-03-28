using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/GameObject and FLoat Event Channel")]
    public class GameObjectFloatChannelSO : ScriptableObject
    {       
        public UnityAction<GameObject, float> GameObjectFloatEvent;

        public void RaiseEvent(GameObject gObject, float value)
        {
            GameObjectFloatEvent?.Invoke(gObject, value);
        }
    }
}

