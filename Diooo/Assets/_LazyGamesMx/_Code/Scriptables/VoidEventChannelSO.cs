//Daniel Navarrete 22/03/23
// This event channel is used to send an event to a listener with a Bool parameter.

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction VoidEvent;

        public void RaiseEvent()
        {
            VoidEvent?.Invoke();
        }
    }
}


