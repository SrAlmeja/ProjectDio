using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MultiplayerGoal : MonoBehaviour
    {
        private Dictionary<AutoData, int> vueltasPorAuto = new Dictionary<AutoData, int>();
        private Dictionary<AutoData, bool> direccionCorrectaPorAuto = new Dictionary<AutoData, bool>();
        [SerializeField] private int vueltasNecesarias;
        private int autosCompletados;

        private void Start()
        {
            AutoData[] autos = FindObjectsOfType<AutoData>();

            foreach (AutoData auto in autos)
            {
                vueltasPorAuto.Add(auto, 0);
                direccionCorrectaPorAuto.Add(auto, true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            AutoData auto = other.GetComponent<AutoData>();

            if (auto != null && direccionCorrectaPorAuto.ContainsKey(auto))
            {
                Vector3 direccionDeColision = other.ClosestPoint(transform.position) - transform.position;
                Vector3 direccionFrontal = transform.forward;

                bool esCaraFrontal = Vector3.Dot(direccionDeColision.normalized, direccionFrontal) > 0.5f;

                if (esCaraFrontal)
                {
                    if (direccionCorrectaPorAuto[auto])
                    {
                        vueltasPorAuto[auto]++;
                    }
                    else
                    {
                        direccionCorrectaPorAuto[auto] = true;
                    }
                }
                else
                {
                    direccionCorrectaPorAuto[auto] = false;
                }

                if (vueltasPorAuto[auto] == vueltasNecesarias)
                {
                    Debug.Log("¡El auto " + auto.name + " ha completado todas las vueltas requeridas!");
                    autosCompletados++;

                    if (autosCompletados == vueltasPorAuto.Count)
                    {
                        OnAllAutosCompleted();
                    }
                }
            }
        }

        private void OnAllAutosCompleted()
        {
            Debug.Log("¡Todos los autos han completado las vueltas requeridas!");
            DioGameManagerMultiplayer.Instance.OnPlayersCompleteRace?.Invoke();
        }
    }
}