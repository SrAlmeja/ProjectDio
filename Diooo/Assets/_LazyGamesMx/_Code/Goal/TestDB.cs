using UnityEngine;
using TMPro;

namespace com.LazyGames.Dio
{
    public class TestDB : MonoBehaviour
    {
        public DatabaseManager databaseManager;
        public string nombre;
        public int carreraId;
        public float tiempo;
        

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                databaseManager.InsertName(nombre);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                databaseManager.InsertTime(carreraId, tiempo);
            }
        }
    }
}