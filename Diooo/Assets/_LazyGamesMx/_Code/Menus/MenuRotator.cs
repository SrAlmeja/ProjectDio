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
        }

        private void RotateMenu(GameObject senderPivot, float position)
        {
            iTween.RotateTo(senderPivot, new Vector3(0f,45f*position,0f), 3f);
        }

        private void MoveMenu(GameObject mainPivot, float position)
        {
            iTween.MoveTo(mainPivot, new Vector3(0f, 0f, 0f), 3f);
        }
    }
}

