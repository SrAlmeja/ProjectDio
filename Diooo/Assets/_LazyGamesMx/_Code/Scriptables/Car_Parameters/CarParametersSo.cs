// Creado Raymundo "CryoStorage" Mosqueda 05/06/2023

using UnityEngine;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/CarParameters")]
    public class CarParametersSo : ScriptableObject
    {
        [Header("CarImpulse Variables")]
        public float ImpulseForce = 9f;
        public float AngleLerpSpeed = 9f;
        public float IndicatorOffset = .3f;
        public float IndicatorRadius = 3f;
        public float FighterRadius = 3f;

        [Header("Car_TimeControl Variables")] 
        public AnimationCurve TargetTimeScale;
        public float FillDuration = 15f;
        public float EmptyDuration = 15f;
        
        [Header("Car_AirControl Variables")]
        public float TorqueForce = 1f;
        public float MaxAngle = 1f;
        public float YOffset = 1f;
        public float RaycastDistance = 1f;
        
        [Header("Car_Respawn Variables")]
        public float MaxHealth = 100f;
        public float RespawnTime = 5f;
        public float PunchDamage = 5f;
        public float DamageCooldown = .3f;
        public float MinDamage = 5f;
        public float MaxDamage = 50f;
    }
}
