using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MenuNavigatonController : MonoBehaviour
    {
        [Header("Channel SO Dependenices")]
        [SerializeField] private GameObjectFloatChannelSO RotationRequestChannel;

        [Header("Dependencies")]
        [SerializeField] private List<GameObject> _menuPivots;

        private GameObject _currentMenu;

        private void Awake()
        {
            _currentMenu = _menuPivots[0];
        }

        private void OnLeftRotationInput()
        {
            RotationRequestChannel.RaiseEvent(_currentMenu, GetMenuPosition() - 1);
        }

        private void OnRightRotationInput()
        {
            RotationRequestChannel.RaiseEvent(_currentMenu, GetMenuPosition() + 1);
        }

        private float GetMenuPosition()
        {
            float rotation = _currentMenu.transform.rotation.y / 45;
            rotation = Mathf.Round(rotation);
            return rotation;
        }
    }
}

