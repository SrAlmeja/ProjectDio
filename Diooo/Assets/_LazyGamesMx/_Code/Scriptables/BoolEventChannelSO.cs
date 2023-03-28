//Daniel Navarrete 22/03/23
// This event channel is used to send an event to a listener with a Bool parameter.

using UnityEngine;
using UnityEngine.Events;


namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public UnityAction<bool> BoolEvent;

        public void RaiseEvent(bool value)
        {
            BoolEvent?.Invoke(value);
        }
    }
}

