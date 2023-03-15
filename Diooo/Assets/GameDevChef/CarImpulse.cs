//Raymundo cryoStorage Mosqueda 07/03/2023
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryoStorage;

namespace com.LazyGames.Dio
{
    public class CarImpulse : MonoBehaviour
    {
        private Rigidbody rb;
        private float impulseAngle = 0f;
        [SerializeField]private float impulseRadius = 4.5f;
        private Vector3 impulsePos;
        private Vector3 impulseDir;

        // Start is called before the first frame update
        void Start()
        {
            Prepare();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void GetDirection()
        {
           impulsePos = CryoMath.PointOnRadius(transform.position, impulseRadius ,impulseAngle);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, impulseRadius);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(impulsePos,.1f);
        }

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                impulseAngle += -.1f;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                impulseAngle += .1f;
            }
        }

        private void Prepare()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    
}
