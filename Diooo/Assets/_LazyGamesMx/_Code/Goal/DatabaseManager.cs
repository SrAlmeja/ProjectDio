using UnityEngine;
using MySql.Data.MySqlClient;
using System.Data;

public class DatabaseManager : MonoBehaviour
{
    private MySqlConnection connection;
    private string connectionString = "Server=sql9.freemysqlhosting.net;Database=sql9622584;Uid=sql9622584;Pwd=SjSaWxf3nE;";

    private void Start()
    {
        ConnectToDatabase();
    }

    void ConnectToDatabase()
    {
        connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("Conexión exitosa a la base de datos.");
            // Puedes ejecutar consultas y realizar operaciones en la base de datos aquí
        }
        catch (MySqlException ex)
        {
            Debug.LogError("Error al conectar a la base de datos: " + ex.Message);
        }
    }

    void OnDestroy()
    {
        if (connection != null && connection.State != ConnectionState.Closed)
        {
            connection.Close();
            Debug.Log("Conexión cerrada.");
        }
    }

    public void SubirNombre(string nombre)
    {
        string query = "INSERT INTO jugadores (nombre) VALUES (@nombre)";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Crear el comando y establecer los parámetros
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);

                    // Ejecutar la consulta
                    command.ExecuteNonQuery();
                }

                Debug.Log("Nombre subido exitosamente a la base de datos.");
            }
            catch (MySqlException ex)
            {
                Debug.LogError("Error al subir el nombre a la base de datos: " + ex.Message);
            }
        }
    }
}