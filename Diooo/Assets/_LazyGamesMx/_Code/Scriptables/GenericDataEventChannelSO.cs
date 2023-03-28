//Daniel Navarrete 22/03/23
// This event channel is used to send an event to a listener with a Bool parameter.

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Generic Data Event Channel")]
    public class GenericDataEventChannelSO : ScriptableObject
    {
        public UnityAction VoidEvent;
        public UnityAction<int> IntEvent;
        public UnityAction<float> FloatEvent;
        public UnityAction<double> DoubleEvent;
        public UnityAction<string> StringEvent;
        public UnityAction<bool> BoolEvent;

        public void RaiseVoidEvent()
        {
            VoidEvent?.Invoke();
        }

        public void RaiseIntEvent(int value)
        {
            IntEvent?.Invoke(value);
        }

        public void RasieFloatEvent(float value)
        {
            FloatEvent?.Invoke(value);
        }

        public void RaiseDoubleEvent(double value)
        {
            DoubleEvent?.Invoke(value);
        }

        public void RaiseStringEvent(string value)
        {
            StringEvent?.Invoke(value);
        }

        public void RaiseBoolEvent(bool value)
        {
            BoolEvent?.Invoke(value);
        }
    }
}

