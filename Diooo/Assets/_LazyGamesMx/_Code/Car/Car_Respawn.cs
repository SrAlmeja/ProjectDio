// Creado Raymundo "CryoStorage" Mosqueda 01/06/2023

using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class Car_Respawn : MonoBehaviour
    {
        [Header("Car Parameters Scriptable Object")]
        [SerializeField] private CarParametersSo carParametersSo;
        
        [Header("Serialized References")]
        [SerializeField] private GameObject healthIndicator;
        [SerializeField]private GameObject visuals;
        private Renderer _healthIndicatorRenderer;

        
        private float _health;
        private float _maxHealth;
        private float _respawnTime;
        private float _elapsedTime;
        private float _damageCooldown;
        private float _minDamage;
        private float _maxDamage;
        private Vector3 _respawnPosition;
        private Quaternion _respawnRotation;
        private Car_Impulse _carImpulse;
        private Rigidbody _rb;
        
        [HideInInspector]public bool isDead;

        private void Start()
        {
            Prepare();
            _health = _maxHealth;
        }

        private void Update()
        {
            CheckHealth();
            
            _elapsedTime += Time.fixedDeltaTime;
            _rb. isKinematic = isDead;
            _healthIndicatorRenderer.material.SetFloat("_FillValue", Remap(_health));
        }

        private void CheckHealth()
        {
            if (_health > 0) return;
            Explode();
        }

        private void OnTriggerEnter(Collider other)
        {
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

        private void OnCollisionEnter(Collision other)
        {
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
            _health -= damage;
            // Debug.Log($"took {damage} dmg");
        }

        float Remap(float value)
        {
            float normalizedValue = Mathf.InverseLerp(0, 100, value);
            return Mathf.Lerp(0, 1, normalizedValue);
        }
        
        private void Respawn()
        {
            isDead = false;
            transform.position = _respawnPosition;
            transform.rotation = _respawnRotation;
            _health = _maxHealth;
            visuals.SetActive(true);
            _rb.velocity = Vector3.zero;
            StopCoroutine(CorWaitToRespawn());
        }

        private void Punched()
        {
            TakeDamage(carParametersSo.PunchDamage);
        }

        private IEnumerator CorWaitToRespawn()
        {
            yield return new WaitForSeconds(_respawnTime);
            Respawn();
        }

        private void Explode()
        {
            isDead = true;
            visuals.SetActive(false);
            // play particle systems
            // play sound
            StartCoroutine(CorWaitToRespawn());
        }

        private void Prepare()
        {
            // Load configurable values from Scriptable Object
            _maxHealth = carParametersSo.MaxHealth;
            _respawnTime = carParametersSo.RespawnTime;
            _respawnPosition = transform.position;
            _damageCooldown = carParametersSo.DamageCooldown;
            _minDamage = carParametersSo.MinDamage;
            _maxDamage = carParametersSo.MaxDamage;
            
            
            _rb = GetComponent<Rigidbody>();
            
            _carImpulse = GetComponent<Car_Impulse>();
            // Subscribe to punch event
            _carImpulse.DoPunchEvent += Punched;
            _healthIndicatorRenderer = healthIndicator.GetComponent<MeshRenderer>();
        }
    }
}
