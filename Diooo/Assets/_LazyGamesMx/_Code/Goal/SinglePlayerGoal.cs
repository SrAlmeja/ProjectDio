using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    
    public class SinglePlayerGoal : MonoBehaviour
    {
        public bool collidedWithFront = false;
        public event Action OnPlayerCrossedGoal;
        private void Start()
        {
            collidedWithFront = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 relativePosition = other.transform.position - transform.position;

            if (Vector3.Dot(relativePosition, transform.forward) > 0)
            {
                if (!collidedWithFront)
                {
                    collidedWithFront = true;
                    return;
                }
                if (collidedWithFront)
                {
                    DioGameManagerSingleplayer.Instance.OnPlayerCrossedGoal(this);
                    OnPlayerCrossedGoal?.Invoke();
                }
            }
            else
            {
                collidedWithFront = false;
            }
        }
    }
}
