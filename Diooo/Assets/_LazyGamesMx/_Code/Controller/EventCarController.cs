//creado raymundo mosqueda 29/03/2023
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio 
{
    [RequireComponent(typeof(Listener))]
    public class EventCarController : GDC_CarController
    {
        private Listener _listener;

        private void Start()
        {
            Prepare();
        }
        
        

        private void Prepare()
        {
            _listener = GetComponent<Listener>();
        }
    }   
}
