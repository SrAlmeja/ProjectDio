//Daniel Navarrete 22/03/23
// This event channel is used to send an event to a listener with a Bool parameter.

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public UnityAction<float> FloatEvent;

        public void RaiseEvent(float value)
        {
            FloatEvent?.Invoke(value);
        }
    }
}
    
