//Fernando Cossio 11/04/23
/// <summary>
/// This will track in what pivot and number we are currently. It will aso request animations to navigate menus. 
/// </summary>

using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace com.LazyGames.Dio
{
    public class MenuNavigator : MonoBehaviour
    {
        [Header("SO Channel Dependencies")]
        [SerializeField] private GameObjectFloatChannelSO _rotationRequestChannel;
        [SerializeField] private GameObjectFloatChannelSO _moveRequestChannel;
        [SerializeField] private VoidEventChannelSO _finishedAnimatingChannel;

        [Header("SO InputChannel Dependencies")]
        [SerializeField] private VoidEventChannelSO UpUIEvent;
        [SerializeField] private VoidEventChannelSO DownUIEvent;
        [SerializeField] private VoidEventChannelSO LeftUIEvent;
        [SerializeField] private VoidEventChannelSO RightUIEvent;
        [SerializeField] private VoidEventChannelSO SelectUIEvent;
        [SerializeField] private VoidEventChannelSO BackUIEvent;

        [Header("Dependencies")]
        [SerializeField] private List<GameObject> _menuPivots;
        [SerializeField] private GameObject _rootPivotGameObject;

        [Header("FMOD Dependencies")]
        [SerializeField] private StudioEventEmitter _generalSFX;
        [SerializeField] private StudioEventEmitter _secondarySFX;
        [SerializeField] private StudioEventEmitter _confirmSFX;
        [SerializeField] private StudioEventEmitter _backSFX;

        [Header("Initial Dependency")]
        [SerializeField] private MenuAdress _currentAdress;

        private bool _enabledInputs = true;

        public MenuAdress CurrentAdress { get { return _currentAdress; } }

        private void OnEnable()
        {
            UpUIEvent.VoidEvent += MoveBackward;
            DownUIEvent.VoidEvent += MoveForward;
            LeftUIEvent.VoidEvent += GoLeftAdress;
            RightUIEvent.VoidEvent += GoRightAdress;
            SelectUIEvent.VoidEvent += MoveForward;
            BackUIEvent.VoidEvent += MoveBackward;
            _finishedAnimatingChannel.VoidEvent += EnableInput;
        }

        private void OnDisable()
        {
            UpUIEvent.VoidEvent -= MoveBackward;   
            DownUIEvent.VoidEvent -= MoveForward;
            LeftUIEvent.VoidEvent -= GoLeftAdress;
            RightUIEvent.VoidEvent -= GoRightAdress;
            SelectUIEvent.VoidEvent -= MoveForward;
            BackUIEvent.VoidEvent -= MoveBackward;
            _finishedAnimatingChannel.VoidEvent -= EnableInput;
        }

        private void MoveForward()
        {
            if (!_enabledInputs) return;
            if (CheckDeadEndForward(_currentAdress))
            {
                _currentAdress.DoForwardAction();
                return;
            }

            RequestNavigation(_currentAdress.ForwardTo);
            _generalSFX.Play();
        }

        private void MoveBackward()
        {
            if (!_enabledInputs) return;
            if (CheckDeadEndBackward(_currentAdress))
            {
                _currentAdress.DoBackAction();
                return;
            }

            RequestNavigation(_currentAdress.BackTo);
            _backSFX.Play();
        }

        private void GoLeftAdress()
        {
            if (!_enabledInputs) return;
            RequestNavigation(_currentAdress.LeftAdress);
        }

        private void GoRightAdress()
        {
            if (!_enabledInputs) return;
            RequestNavigation(_currentAdress.RightAdress);
        }

        private void RequestNavigation(MenuAdress toAdress)
        {
            if (CheckNullAdress(toAdress)) return;
            if (toAdress == _currentAdress) return;

            _enabledInputs = false;
            _moveRequestChannel.GameObjectFloatEvent(_rootPivotGameObject, toAdress.Pivot);
            _rotationRequestChannel.GameObjectFloatEvent(_menuPivots[toAdress.Pivot], toAdress.Number);
            ChangeAdress(toAdress);
        }

        private void ChangeAdress(MenuAdress newAdress)
        {
            _currentAdress = newAdress;
            //Debug.Log("Current Adress is: Pivot: " + newAdress.Pivot + " Number: " + newAdress.Number);
        }

        private bool CheckDeadEndForward(MenuAdress adress)
        {
            return adress.ForwardTo == null;
        }

        private bool CheckDeadEndBackward(MenuAdress adress)
        {
            return adress.BackTo == null;
        }

        private bool CheckNullAdress(MenuAdress adress)
        {
            if(adress == null)
            {
                return true;
            }
            return false;
        }

        private void EnableInput()
        {
            _enabledInputs = true;
        }


    }
}
