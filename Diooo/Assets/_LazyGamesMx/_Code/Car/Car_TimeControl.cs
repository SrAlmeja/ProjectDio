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
                currentTimeScale = targetTimeScale;
                currentTimeScale = Mathf.Epsilon> Mathf.Abs(targetTimeScale-.1f) ? 1 : .01f;
                NormalizeDeltaTime(normalizeFactor);
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
        
        private void ChangeTimeScale(float value)
        {
            targetTimeScale = value;
        }
    }
    
}
