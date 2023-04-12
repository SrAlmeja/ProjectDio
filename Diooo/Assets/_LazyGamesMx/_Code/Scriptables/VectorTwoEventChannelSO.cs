//Daniel Navarrete 12/04/23
// This event channel is used to send an event to a listener with a Vector2 parameter.

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Vector2 Event Channel")]
    public class VectorTwoEventChannelSO : ScriptableObject
    {
        public UnityAction<Vector2> Vector2Event;

        public void RaiseEvent(Vector2 value)
        {
            Vector2Event?.Invoke(value);
        }
    }
}