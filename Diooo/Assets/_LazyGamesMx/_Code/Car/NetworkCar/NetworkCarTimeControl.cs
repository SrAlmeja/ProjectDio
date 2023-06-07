using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class NetworkCarTimeControl : NetworkBehaviour
    {
        [Header("Time Control")] 
        [SerializeField] private bool doSlow;
        [SerializeField] private float targetTimeScale;
        
        private NetworkSteeringEventsListener _listener;
        private float currentTimeScale = 1;
        private float savedMagnitude;
        private readonly float normalizeFactor = .02f;

        public override void OnNetworkSpawn()
        {
            if(!IsOwner) return;
            Prepare();
            base.OnNetworkSpawn();
        }

        private void Update()
        {
            if(!IsOwner) return;
            Slow();
            Time.timeScale = currentTimeScale;
        }

        void Slow()
        {
            if(!IsOwner) return;
            // doSlow = _listener.stopTime;
            switch (doSlow)
            {
                case true:
                    currentTimeScale = targetTimeScale;
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = false;
                    break;
                case false:
                    currentTimeScale = 1f;
                    NormalizeDeltaTime(normalizeFactor);
                    doSlow = true;
                    break;
            }
        }

        private void NormalizeDeltaTime(float factor)
        {
            if(!IsOwner) return;
            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = Time.timeScale * factor;
        
        }
        private void Prepare()
        {
            if(!IsOwner) return;
            _listener = GetComponent<NetworkSteeringEventsListener>();

        }
    }
}
