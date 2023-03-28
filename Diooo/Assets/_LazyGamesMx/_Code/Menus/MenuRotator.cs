using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MenuRotator : MonoBehaviour
    {
        [Header("Channel SO Dependencies")]
        [SerializeField] private GameObjectFloatChannelSO RotationRequestChannel;

        private void OnEnable()
        {
            RotationRequestChannel.GameObjectFloatEvent += RotateObject;
        }
        private void OnDisable()
        {
            RotationRequestChannel.GameObjectFloatEvent -= RotateObject;
        }

        private void RotateObject(GameObject senderPivot, float position)
        {
            iTween.RotateTo(senderPivot, new Vector3(0f,45f*position,0f), 3f);
        }
    }
}

