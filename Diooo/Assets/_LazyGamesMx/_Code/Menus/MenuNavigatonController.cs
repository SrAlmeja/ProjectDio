//Fernando Cossio 28/03/2023
/// <summary>
/// This Script takes cares of menu navigation. Using the current active menu object it will determine which menu should go next.
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MenuNavigatonController : MonoBehaviour
    {
        [Header("Channel SO Dependenices")]
        [SerializeField] private GameObjectFloatChannelSO RotationRequestChannel;
        [SerializeField] private GameObjectFloatChannelSO MoveRequestChannel;

        [Header("Dependencies")]
        [SerializeField] private List<GameObject> _menuPivots;

        private GameObject _currentMenu;

        private void Awake()
        {
            _currentMenu = _menuPivots[0];
        }

        private void OnUpMoveInput()
        {
            MoveRequestChannel.RaiseEvent(_currentMenu, GetMenuPosition());
        }

        private void OnDownMoveInput()
        {
            MoveRequestChannel.RaiseEvent(_currentMenu, GetMenuPosition());
        }

        private void OnLeftRotationInput()
        {
            RotationRequestChannel.RaiseEvent(_currentMenu, GetMenuRotationPosition() - 1);
        }

        private void OnRightRotationInput()
        {
            RotationRequestChannel.RaiseEvent(_currentMenu, GetMenuRotationPosition() + 1);
        }

        private float GetMenuRotationPosition()
        {
            float rotation = _currentMenu.transform.rotation.y / 45;
            rotation = Mathf.Round(rotation);
            return rotation;
        }

        private float GetMenuPosition()
        {
            float position = _currentMenu.transform.position.x; //TODO
            position = Mathf.Round(position);
            return position;
        }
    }
}

