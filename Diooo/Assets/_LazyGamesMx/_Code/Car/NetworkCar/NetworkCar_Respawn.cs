using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using Unity.Netcode;
using UnityEngine;

public class NetworkCar_Respawn : NetworkBehaviour
{
        [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;
        
        [Header("Serialized References")]
        [SerializeField] private GameObject healthIndicator;
        [SerializeField]private GameObject visuals;
        
        [HideInInspector]public bool isDead;

        private float _healthLerpSpeed = 3f;
        private float _currentHealth;
        private float _targetHealth;
        private float _maxHealth;
        private float _respawnTime;
        private float _elapsedTime;
        private float _damageCooldown;
        private float _minDamage;
        private float _maxDamage;
        private Vector3 _respawnPosition;
        private Quaternion _respawnRotation;
        
        private NetworkCarImpulse _carImpulse;
        private Rigidbody _rb;
        private Renderer _healthIndicatorRenderer;
        private NetworkCarParticles _carParticlesManager;
        
        public event System.Action OnDie;
        public event System.Action OnRespawn;


        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            
            Prepare();
            _targetHealth = _maxHealth;
        }


        private void Update()
        {
            if (!IsOwner) return;

            CheckHealth();
            _currentHealth = LerpHealth();
            _elapsedTime += Time.fixedDeltaTime;
            _rb. isKinematic = isDead;
            _healthIndicatorRenderer.material.SetFloat("_FillValue", Remap(_currentHealth));
        }

        private void CheckHealth()
        {
            if (_targetHealth > 0) return;
            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!IsOwner) return;

            if (!other.CompareTag("CheckPoint")) return;
            Transform t = other.transform;
            UpdateCheckPoint(t.position, t.rotation);
        }

        private void UpdateCheckPoint(Vector3 pos, Quaternion rot)
        {
            _respawnPosition = pos;
            _respawnRotation = rot;
            // Debug.Log($"updated checkpoint to {pos}");
        }
        
        private float LerpHealth()
        {
            float result = Mathf.LerpAngle(_currentHealth, _targetHealth, _healthLerpSpeed * Time.fixedUnscaledDeltaTime);
            return result;
        }

        private void OnCollisionEnter(Collision other)
        {
            if(!IsOwner) return;
            _carParticlesManager.PlaySparksParticle(other.contacts[0].point);
            if (_elapsedTime < _damageCooldown) return;
            float mag  = other.relativeVelocity.magnitude;
            TakeDamage(CalculateDamage(mag));
            _elapsedTime = 0;
        }
        
        private float CalculateDamage(float mag)
        {
            float dmg = mag * 0.5f;
            switch (dmg)
            {
                case float f when (f < _minDamage):
                    return 1;
                
                case float f when (f > _maxDamage):
                    return _maxDamage;
                
                default: return dmg;
            }
        }

        private void TakeDamage(float damage)
        {
            _targetHealth -= damage;
            // Debug.Log($"took {damage} dmg");
        }

        float Remap(float value)
        {
            float normalizedValue = Mathf.InverseLerp(0, 100, value);
            return Mathf.Lerp(0, 1, normalizedValue);
        }
        
        private void Respawn()
        {
            if (!IsOwner) return;
            OnRespawn?.Invoke();
            isDead = false;
            transform.position = _respawnPosition;
            transform.rotation = _respawnRotation;
            _targetHealth = _maxHealth;
            visuals.SetActive(true);
            _rb.velocity = Vector3.zero;
            StopCoroutine(CorWaitToRespawn());
        }

        private void Punched()
        {
            _carParticlesManager.PlaySparksParticle(transform.position);
            TakeDamage(carParametersSo.PunchDamage);
        }

        private IEnumerator CorWaitToRespawn()
        {
            OnDie?.Invoke();
            yield return new WaitForSeconds(_respawnTime);
            Respawn();
        }

        private void Explode()
        {
            isDead = true;
            visuals.SetActive(false);
            // play sound
            StartCoroutine(CorWaitToRespawn());
        }

        private void Prepare()
        {
            // Load configurable values from Scriptable Object
            _maxHealth = carParametersSo.MaxHealth;
            _healthLerpSpeed = carParametersSo.HealthLerpSpeed;
            _respawnTime = carParametersSo.RespawnTime;
            _respawnPosition = transform.position;
            _damageCooldown = carParametersSo.DamageCooldown;
            _minDamage = carParametersSo.MinDamage;
            _maxDamage = carParametersSo.MaxDamage;

            _rb = GetComponent<Rigidbody>();
            _carImpulse = GetComponent<NetworkCarImpulse>();
            _healthIndicatorRenderer = healthIndicator.GetComponent<MeshRenderer>();
            _carParticlesManager = GetComponent<NetworkCarParticles>();
            
            // Subscribe to punch event
            _carImpulse.DoPunchEvent += Punched;
        }
        
}
