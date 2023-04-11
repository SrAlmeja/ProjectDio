//Fernando Cossio 11/04/23
/// <summary>
/// This is used like a individual house adress. It reside on a street (pivot). It point to where it wants to go back and ford.
/// </summary>

using UnityEngine;
using UnityEngine.Events;

namespace com.LazyGames.Dio
{
    public class MenuAdress : MonoBehaviour
    {
        [Header("Adress")]
        [SerializeField] private int _pivot;
        [SerializeField] private int _number;
        [SerializeField] private MenuAdress _forwardTo;
        [SerializeField] private MenuAdress _backTo;

        public UnityEvent ForwardAction;
        public UnityEvent BackAction;

        public int Pivot { get { return _pivot; }}
        public int Number { get { return _number;} }
        public MenuAdress ForwardTo { get {  return _forwardTo; }}
        public MenuAdress BackTo { get { return _backTo; }}

        public void DoForwardAction()
        {
            ForwardAction?.Invoke();
        }

        public void DoBackAction()
        {
            BackAction?.Invoke();
        }
    }
}

