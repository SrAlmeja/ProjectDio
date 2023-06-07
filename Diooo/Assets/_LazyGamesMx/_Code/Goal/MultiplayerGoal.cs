using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MultiplayerGoal :  MonoBehaviour
    {
        
        // public override void OnNetworkSpawn()
        // {
        //     DioGameManagerMultiplayer.Instance.OnGameStateChange += HandleGameStateChange;
        // }

        // private void HandleGameStateChange(DioGameManagerMultiplayer.GameStates state)
        // {
        //     if (state == DioGameManagerMultiplayer.GameStates.GamePlaying)
        //     {
        //
        //     }
        // }
        
        private void OnTriggerEnter(Collider other)
        {
            NetworkCarData networkCar = other.GetComponent<NetworkCarData>();
            if (networkCar != null) 
            { 
                Debug.Log("El auto = " + networkCar.OwnerClientId + " ha cruzado la meta"); 
                DioGameManagerMultiplayer.Instance.OnPlayerCompletedRace?.Invoke(true);
            }
            
            // Debug.Log("Auto pasa triguer");
            
            // Vector3 direccionDeColision = other.ClosestPoint(transform.position) - transform.position;
            // Vector3 direccionFrontal = transform.forward;
            //
            // bool esCaraFrontal = Vector3.Dot(direccionDeColision.normalized, direccionFrontal) > 0.5f;
            //
            // if (esCaraFrontal)
            // {
            
        }

        // private void OnAllAutosCompleted()
        // {
        //     DioGameManagerMultiplayer.Instance.OnAllPlayersCompleteRace?.Invoke();
        // }
        
    }
}