using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _actualLap;
        [SerializeField] private BoolEventChannelSO _isReverse;
        private bool _reverse = false;

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
                    if (_reverse) 
                    {
                        _isReverse.BoolEvent(false);
                        _reverse = false;
                        return;
                    }
                    else if(!_reverse)
                    {
                        _actualLap.VoidEvent();
                    }
                }
                else
                {
                    _isReverse.BoolEvent(true);
                    _reverse = true;
                }
            }
        }
    }
}