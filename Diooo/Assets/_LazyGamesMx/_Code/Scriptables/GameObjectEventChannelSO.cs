//Daniel Navarrete 22/03/23
// This event channel is used to send an event to a listener with a Bool parameter.

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/GameObject Event Channel")]
    public class GameObjectEventChannelSO : ScriptableObject
    {
        public UnityAction<GameObject> GameObjectEvent;

        public void RaiseEvent(GameObject value)
        {
            GameObjectEvent?.Invoke(value);
        }
    }
}
    
