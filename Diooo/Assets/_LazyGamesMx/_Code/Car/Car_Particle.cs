// Creado Raymundo "CryoStorage" Mosqueda 01/06/2023

using System;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Car_Particle : MonoBehaviour
    {
        private ParticleSystem particleSystem;
        
        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        
        public void PlayParticleAtLocation(Vector3 pos)
        {
            transform.position = pos;
            particleSystem.Play();
        }
    }
    
}
