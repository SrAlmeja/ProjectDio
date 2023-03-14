using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/VoidEventChanel")]
public class VoidEventScript : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
