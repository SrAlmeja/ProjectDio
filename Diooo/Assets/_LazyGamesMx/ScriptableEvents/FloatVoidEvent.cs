using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/FloatEvent")]
public class FloatVoidEvent : ScriptableObject
{
    public UnityAction<float> OnEventRaised;

    public void RaiseEvent(float f)
    {
        OnEventRaised?.Invoke(f);
    }
}
