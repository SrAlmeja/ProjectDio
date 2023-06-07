using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MultiplayerGoal :  NetworkBehaviour
    {
        

        public override void OnNetworkSpawn()
        {
            DioGameManagerMultiplayer.Instance.OnGameStateChange += HandleGameStateChange;
        }

        private void HandleGameStateChange(DioGameManagerMultiplayer.GameStates state)
        {
            if (state == DioGameManagerMultiplayer.GameStates.GamePlaying)
            {

            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            NetworkCarData networkCar = other.GetComponent<NetworkCarData>();
            
            if (networkCar != null)
            {
                Debug.Log("El auto " + networkCar.OwnerClientId + " ha completado todas las vueltas requeridas");
                DioGameManagerMultiplayer.Instance.OnPlayerFinishRace?.Invoke(networkCar.OwnerClientId);
            }
            
            
            // Debug.Log("<color=#DFFE97>On trigger + = </color>" +  auto.name);
            //
            //
            // if (auto != null && direccionCorrectaPorAuto.ContainsKey(auto))
            // {
            //     Vector3 direccionDeColision = other.ClosestPoint(transform.position) - transform.position;
            //     Vector3 direccionFrontal = transform.forward;
            //
            //     bool esCaraFrontal = Vector3.Dot(direccionDeColision.normalized, direccionFrontal) > 0.5f;
            //
            //     if (esCaraFrontal)
            //     {
            //         if (direccionCorrectaPorAuto[auto])
            //         {
            //             vueltasPorAuto[auto]++;
            //         }
            //         else
            //         {
            //             direccionCorrectaPorAuto[auto] = true;
            //         }
            //     }
            //     else
            //     {
            //         direccionCorrectaPorAuto[auto] = false;
            //     }
            //
            //     if (vueltasPorAuto[auto] == vueltasNecesarias)
            //     {
            //         Debug.Log("El auto " + auto.name + " ha completado todas las vueltas requeridas");
            //         autosCompletados++;
            //
            //         if (autosCompletados == vueltasPorAuto.Count)
            //         {
            //             if(IsServer)
            //                 OnAllAutosCompleted();
            //         }
            //     }
            // }
        }

        private void OnAllAutosCompleted()
        {
            DioGameManagerMultiplayer.Instance.OnAllPlayersCompleteRace?.Invoke();
        }
        
    }
}