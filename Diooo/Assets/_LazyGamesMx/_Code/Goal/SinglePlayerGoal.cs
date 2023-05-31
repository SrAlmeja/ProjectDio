using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    
    public class SinglePlayerGoal : MonoBehaviour
    {
        public bool collidedWithFront = false;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] SceneKeySO sceneKeySo;

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
                    Debug.Log("Next Stage");
                    
                    // sceneLoader.LoadScene(sceneKeySo);
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
