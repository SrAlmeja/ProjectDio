//Raymundo cryoStorage Mosqueda 07/03/2023
//
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class Car_TimeControl : MonoBehaviour
    {
        [Header("TimeControl")] 
        [SerializeField] private bool doSlow;
        [SerializeField] private float targetTimeScale;
        
        
        private float currentTimeScale = 1;
        private float savedMagnitude;
        private readonly float normalizeFactor = .02f;


        private void Update()
        {
            Slow();
            Time.timeScale = currentTimeScale;
        }

        void Slow()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                switch (doSlow)
                {
                    case true:
                        currentTimeScale = targetTimeScale;
                        NormalizeDeltaTime(normalizeFactor);
                        doSlow = false;
                        break;
                    case false:
                        currentTimeScale = 1f;
                        NormalizeDeltaTime(normalizeFactor);
                        doSlow = true;
                        break;
                }
                // currentTimeScale = targetTimeScale;
                // currentTimeScale = .33f> Mathf.Abs(targetTimeScale-.1f) ? 1 : .01f;
            }
        }

        private void NormalizeDeltaTime(float factor)
        {
            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = Time.timeScale * factor;
        }

        public void DebuggingInput(InputAction.CallbackContext context)
        {
        //     if (context.started)
        //     {  
        //         switch (doSlow)
        //         {
        //             case false:
        //                 doSlow = true;
        //                 ChangeTimeScale(1f);
        //                 break;
        //             case true:
        //                 doSlow = false;
        //                 ChangeTimeScale(0f);
        //                 break;
        //         }
        //     }
        }
    }
    
}
