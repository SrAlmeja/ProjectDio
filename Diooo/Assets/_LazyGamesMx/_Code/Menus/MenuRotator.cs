//Fernando Cossio 28/03/2023
/// <summary>
/// This script handles all movement requiered by the main menu. 
/// </summary>

using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MenuRotator : MonoBehaviour
    {
        [Header("Channel SO Dependencies")]
        [SerializeField] private GameObjectFloatChannelSO _rotationRequestChannel;
        [SerializeField] private GameObjectFloatChannelSO _moveRequestChannel;
        [SerializeField] private VoidEventChannelSO _finishedAnimatingChannel;

        private int _currentAnimations = 0;

        private void OnEnable()
        {
            _rotationRequestChannel.GameObjectFloatEvent += RotateMenu;
            _moveRequestChannel.GameObjectFloatEvent += MoveMenu;
        }
        private void OnDisable()
        {
            _rotationRequestChannel.GameObjectFloatEvent -= RotateMenu;
            _moveRequestChannel.GameObjectFloatEvent -= MoveMenu;
        }

        private void RotateMenu(GameObject senderPivot, float position)
        {
            _currentAnimations++;
            iTween.RotateTo(senderPivot, iTween.Hash(
                "rotation", new Vector3(0f, 45f * position, 0f),
                "time", 1f, 
                "easetype", iTween.EaseType.easeInOutSine,
                "looptype", iTween.LoopType.none, 
                "oncomplete","Finished",
                "oncompletetarget", this.gameObject));
        }

        private void MoveMenu(GameObject mainPivot, float position)
        {
            _currentAnimations++;
            iTween.MoveTo(mainPivot, iTween.Hash(
               "position", new Vector3(0f, 5f * position, 0f),
               "time", 1f,
               "easetype", iTween.EaseType.easeInOutSine,
               "looptype", iTween.LoopType.none,
               "oncomplete","Finished",
               "oncompletetarget", this.gameObject)); 
        }

        private void Finished()
        {
            _currentAnimations--;

            if(_currentAnimations == 0)
            {
                _finishedAnimatingChannel.VoidEvent();
            }
        }
    }
}

