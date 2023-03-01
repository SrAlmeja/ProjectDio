using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class ConnectionNetworking : MonoBehaviour
    {

        private bool _hasInternet;

        public bool HasInternet => _hasInternet;

    private void Awake()
        {
        }

        void Start()
        {
            InitializerNetworking.Instance.OnFinishLoading += Initialize;
        }

        void Update()
        {

        }

        void Initialize()
        {
            CheckConnection();
        }

        void CheckConnection()
        {
            
        }

    }
}