// Creado Raymundo Mosqueda 24/04/2023
//
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    public class NetworkCameraFollow : NetworkBehaviour
    {
        // [Header("Configurable Values")]
        [SerializeField] private Vector3 offset = new Vector3(0,4,-10);
        [SerializeField] private float translateSpeed = 10;
        [SerializeField] private float rotationSpeed = 10;

        [HideInInspector]public bool hasTarget;
        [SerializeField]private Transform _target;

        public void SetTarget(Transform target)
        {   
            if (!IsOwner) return;
            Debug.Log("<color=#DDABFF> Target to car player = </color>" + target.name);
            _target = target.transform; 
            hasTarget = true;
        }

        private void FixedUpdate()
        {
            if (!hasTarget) return;
            if (!IsOwner) return;
            HandleTranslation();
            HandleRotation();
        }

        public override void OnNetworkSpawn()
        {
            gameObject.SetActive(IsOwner);
            ReceiveClient();
        }

        private void HandleTranslation()
        {
            if(!IsOwner) return;
            var targetPosition = _target.TransformPoint(offset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
        }
        private void HandleRotation()
        {
            if(!IsOwner) return;
            var direction = _target.position - transform.position;
            var rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        private void ReceiveClient()
        {
            if (!IsOwner) return;
            var target = OwnerClientId;
            Debug.Log("<color=#DDABFF> ReceiveClient </color>" + target);
            var player = NetworkManager.Singleton.ConnectedClients[target].PlayerObject;
            Debug.Log("<color=#DDABFF> ReceiveClient </color>" + player.name);
            var playerTransform = player.GetComponent<Transform>();
            SetTarget(playerTransform);
        }
        
    }
}
