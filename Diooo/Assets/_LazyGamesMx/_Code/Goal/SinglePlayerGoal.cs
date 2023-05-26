using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class SinglePlayerGoal : MonoBehaviour
    {
        private bool colisionTrasera = true;

        private void OnTriggerEnter(Collider other)
        {
            Vector3 direccion = other.transform.position - transform.position;
            float dotProduct = Vector3.Dot(direccion, transform.forward);

            if (dotProduct < 0f)
            {
                if (colisionTrasera)
                {
                    Debug.Log("Carrera completada");
                }
                colisionTrasera = true;
            }
            else
            {
                if (!colisionTrasera)
                {
                    return; // Retorna sin ejecutar más código si colisiona con la cara frontal y el booleano es falso
                }

                colisionTrasera = false;
            }
        }
    }
}
