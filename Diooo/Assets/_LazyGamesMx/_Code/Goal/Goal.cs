using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _actualLap;
        private bool _isReverse = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                BoxCollider boxCollider = GetComponent<BoxCollider>();

                Vector3 closestPoint = boxCollider.ClosestPoint(other.transform.position);

                Vector3 direction = closestPoint - transform.position;

                Vector3 frontDirection = transform.forward;

                float dotProduct = Vector3.Dot(direction, frontDirection);

                float threshold = 0.0f;

                if (dotProduct >= threshold)
                {
                    Debug.Log("Se está tocando la cara frontal del objeto");
                }
                else
                {
                    Debug.Log("Se está tocando la cara trasera del objeto");
                }
            }
        }
    }
}