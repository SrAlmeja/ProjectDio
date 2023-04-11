//Creado Raymundo Mosqueda 

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Container/Steering Event Container")]
    public class EventsContainerSO : ScriptableObject
    {
        [SerializeField] private ScriptableObject _handBrakeEvent;
        [SerializeField] private ScriptableObject _stopTimeEvent;
        [SerializeField] private ScriptableObject _angleEvent;
        [SerializeField] private ScriptableObject _torqueEvent;
    }
}
