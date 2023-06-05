using UnityEngine;

namespace com.LazyGames.Dio
{
    public class CameraInputListener : MonoBehaviour
    {
        [SerializeField] private VectorTwoEventChannelSO _vector2Camera;

        private void OnEnable()
        {
            _vector2Camera.Vector2Event += CameraMove;
        }

        private void OnDisable()
        {
            _vector2Camera.Vector2Event -= CameraMove;
        }

        void CameraMove(Vector2 cameraPosition)
        {
            Debug.Log("vector 2" + cameraPosition);
        }
    }
}

