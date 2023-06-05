using UnityEngine;

public class TestDB : MonoBehaviour
{
    public DatabaseManager databaseManager;
    public string nombre;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            databaseManager.SubirNombre(nombre);
        }
    }
}
