//Daniel Navarrete 22/03/23
// This event channel is used to send an event to a listener with a Bool parameter.

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Int Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public UnityAction<int> IntEvent;
        public UnityAction<int, int> DoubleIntEvent;

        public void RaiseEvent(int value)
        {
            IntEvent?.Invoke(value);
        }

        public void RaiseDoubleEvent(int valueOne, int valueTwo)
        {
            DoubleIntEvent?.Invoke(valueOne, valueTwo);
        }
    }
}

