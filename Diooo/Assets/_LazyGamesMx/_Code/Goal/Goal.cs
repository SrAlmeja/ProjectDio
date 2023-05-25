using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _actualLap;
        [SerializeField] private BoolEventChannelSO _isReverse;
        private bool _reverse = false;
        private Dictionary<GameObject, bool> autosDictionary;

        void Start()
        {
            autosDictionary = new Dictionary<GameObject, bool>();

            GameObject[] autos = GameObject.FindGameObjectsWithTag("Player");

            for (int i = 0; i < autos.Length; i++)
            {
                GameObject auto = autos[i];
                bool estadoInicial = true; 

                autosDictionary.Add(auto, estadoInicial);
            }

            foreach (KeyValuePair<GameObject, bool> pair in autosDictionary)
            {
                GameObject auto = pair.Key;
                bool estado = pair.Value;

                int numeroAuto = auto.GetInstanceID();
                Debug.Log("Auto: " + numeroAuto + ", Estado: " + estado);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            GameObject objetoColisionado = other.gameObject;

            // Verificar si el objeto está en el diccionario
            if (autosDictionary.ContainsKey(objetoColisionado))
            {
                // Cambiar el valor booleano del objeto
                autosDictionary[objetoColisionado] = false;

                // Realizar otras acciones o lógica aquí según sea necesario

                // Ejemplo: Imprimir el estado actual del objeto
                bool estadoObjeto = autosDictionary[objetoColisionado];
                Debug.Log("Estado del objeto: " + estadoObjeto);
            }

            if (other.CompareTag("Player"))
            {
                BoxCollider boxCollider = GetComponent<BoxCollider>();

                Vector3 closestPoint = boxCollider.ClosestPoint(other.transform.position);

                Vector3 direction = closestPoint - transform.position;

                Vector3 frontDirection = transform.forward;

                float dotProduct = Vector3.Dot(direction, frontDirection);

                float threshold = 0.0f;

                if (dotProduct >= threshold)
                {
                    if (_reverse) 
                    {
                        _isReverse.BoolEvent(false);
                        _reverse = false;
                        return;
                    }
                    else if(!_reverse)
                    {
                        _actualLap.VoidEvent();
                    }
                }
                else
                {
                    _isReverse.BoolEvent(true);
                    _reverse = true;
                }
            }
        }
    }
}