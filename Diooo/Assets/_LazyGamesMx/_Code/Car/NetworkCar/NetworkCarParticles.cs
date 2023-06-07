using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCarParticles : NetworkBehaviour
{
        [Header("Objects with CarParticle component")]   
        [SerializeField] private ParticleSystem sparksParticle;

        [Header("Explosion Particles Systems")]
        [SerializeField] private ParticleSystem explosionSmokeParticle;
        [SerializeField] private ParticleSystem explosionFireParticle;
        [SerializeField] private ParticleSystem explosionExplosionParticle;
        [SerializeField] private ParticleSystem fireParticle;
        
        private NetworkCar_Respawn _carRespawn;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            Prepare();
        }
        
        public void PlaySparksParticle(Vector3 pos)
        {
            if (!IsOwner) return;

            sparksParticle.transform.position = pos;
            sparksParticle.Play();
        }
        
        private void PlayExplosionParticles()
        {
            if (!IsOwner) return;

            explosionSmokeParticle.Play();
            explosionFireParticle.Play();
            explosionExplosionParticle.Play();
            fireParticle.Play();
        }
        
        private void StopExplosionParticles()
        {
            if (!IsOwner) return;

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
            if (!IsOwner) return;

            _carRespawn = GetComponent<NetworkCar_Respawn>();
            _carRespawn.OnDie += PlayExplosionParticles;
            _carRespawn.OnRespawn += StopExplosionParticles;
        }
    }
