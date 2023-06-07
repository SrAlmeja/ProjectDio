// Creado Raymundo "CryoStorage" Mosqueda 05/06/2023

using System;
using UnityEngine;


namespace com.LazyGames.Dio
{
    public class CarParticlesManager : MonoBehaviour
    {
        [Header("Objects with CarParticle component")]   
        [SerializeField] private ParticleSystem sparksParticle;

        [Header("Explosion Particles Systems")]
        [SerializeField] private ParticleSystem explosionSmokeParticle;
        [SerializeField] private ParticleSystem explosionFireParticle;
        [SerializeField] private ParticleSystem explosionExplosionParticle;
        [SerializeField] private ParticleSystem fireParticle;
        
        private Car_Respawn _carRespawn;
        private void Start()
        {
            Prepare();
        }

        public void PlaySparksParticle(Vector3 pos)
        {
            sparksParticle.transform.position = pos;
            sparksParticle.Play();
        }
        
        private void PlayExplosionParticles()
        {
            explosionSmokeParticle.Play();
            explosionFireParticle.Play();
            explosionExplosionParticle.Play();
            fireParticle.Play();
        }
        
        private void StopExplosionParticles()
        {
            explosionSmokeParticle.Stop();
            explosionFireParticle.Stop();
            explosionExplosionParticle.Stop();
            fireParticle.Stop();
            
            explosionFireParticle.Clear();
            explosionExplosionParticle.Clear();
            explosionSmokeParticle.Clear();
            fireParticle.Clear();
        }

        private void Prepare()
        {
            _carRespawn = GetComponent<Car_Respawn>();
            _carRespawn.OnDie += PlayExplosionParticles;
            _carRespawn.OnRespawn += StopExplosionParticles;
        }
    }
}
