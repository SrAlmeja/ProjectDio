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
        [SerializeField] private GameObjectFloatChannelSO RotationRequestChannel;
        [SerializeField] private GameObjectFloatChannelSO MoveRequestChannel;

        private void OnEnable()
        {
            RotationRequestChannel.GameObjectFloatEvent += RotateMenu;
            MoveRequestChannel.GameObjectFloatEvent += MoveMenu;
        }
        private void OnDisable()
        {
            RotationRequestChannel.GameObjectFloatEvent -= RotateMenu;
            MoveRequestChannel.GameObjectFloatEvent -= MoveMenu;
        }

        private void RotateMenu(GameObject senderPivot, float position)
        {
            iTween.RotateTo(senderPivot, new Vector3(0f,45f*position,0f), 3f);
        }

        private void MoveMenu(GameObject mainPivot, float position)
        {
            iTween.MoveAdd(mainPivot, new Vector3(0f, 5f*position, 0f), 3f);
        }
    }
}

