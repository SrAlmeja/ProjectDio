using UnityEngine;
using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace com.LazyGames.Dio
{
    public class DatabaseManager : MonoBehaviour
    {
        [SerializeField] private IntValue intValue;

        private MySqlConnection _connection;
        private string _connectionString = "Server=sql9.freemysqlhosting.net;Database=sql9622584;Uid=sql9622584;Pwd=SjSaWxf3nE;";

        private void Start()
        {
            ConnectToDatabase();
        }

        void ConnectToDatabase()
        {
            _connection = new MySqlConnection(_connectionString);
            try
            {
                _connection.Open();
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
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                Debug.Log("Conexión cerrada.");
            }
        }

        public void InsertName(string nombre)
        {
            intValue.value = -1; // Variable para almacenar el jugador_id

            // Consulta para verificar si el nombre ya existe y obtener el jugador_id
            string selectQuery = "SELECT jugador_id FROM jugadores WHERE nombre = @nombre";

            // Consulta para insertar el nuevo registro y obtener el jugador_id si no existe el nombre
            string insertQuery = "INSERT INTO jugadores (nombre) VALUES (@nombre); SELECT LAST_INSERT_ID();";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificar si el nombre ya existe
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@nombre", nombre);

                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // El nombre ya existe, obtener el jugador_id
                                intValue.value = reader.GetInt32("jugador_id");
                            }
                        }
                    }

                    // Si el jugador_id es -1, significa que el nombre no existe y se debe insertar
                    if (intValue.value == -1)
                    {
                        // Insertar el nuevo registro
                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@nombre", nombre);

                            // Ejecutar la consulta de inserción y obtener el jugador_id
                            intValue.value = Convert.ToInt32(insertCommand.ExecuteScalar());
                        }
                    }

                    Debug.Log("Nombre subido exitosamente a la base de datos. jugador_id: " + intValue.value);
                }
                catch (MySqlException ex)
                {
                    Debug.LogError("Error al subir el nombre a la base de datos: " + ex.Message);
                }
            }
        }

        public void InsertTime(int carreraId, float tiempo)
        {
            DateTime fechaActual = DateTime.Now.Date;

            string query = "INSERT INTO tiempos (jugador_id, carrera_id, tiempo, fecha_actual) VALUES (@jugadorId, @carreraId, @tiempo, @fecha_actual)";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    // Crear el comando y establecer los parámetros
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@jugadorId", intValue.value);
                        command.Parameters.AddWithValue("@carreraId", carreraId);
                        command.Parameters.AddWithValue("@tiempo", tiempo);
                        command.Parameters.AddWithValue("@fecha_actual", fechaActual);

                        // Ejecutar la consulta
                        command.ExecuteNonQuery();
                    }

                    Debug.Log("Tiempo insertado exitosamente en la base de datos.");
                }
                catch (MySqlException ex)
                {
                    Debug.LogError("Error al insertar el tiempo en la base de datos: " + ex.Message);
                }
            }
        }

        public string GetTop10Names()
        {
            string query = "SELECT nombre_jugador, nombre_carrera, tiempo FROM vista_tiempos ORDER BY tiempo ASC LIMIT 10";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            string top10Name = "Player\n\n";

                            while (reader.Read())
                            {
                                string nombreJugador = reader.GetString("nombre_jugador");

                                top10Name += nombreJugador + "\n";
                            }

                            return top10Name;
                        }
                    }
                }
                catch
                {
                    string DBError = "Database Error";
                    return DBError;
                }
            }
        }

        public string GetTop10Race()
        {
            string query = "SELECT nombre_jugador, nombre_carrera, tiempo FROM vista_tiempos ORDER BY tiempo ASC LIMIT 10";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            string top10Race = "Race\n\n";


                            while (reader.Read())
                            {
                                string nombreCarrera = reader.GetString("nombre_carrera");

                                top10Race += nombreCarrera + "\n";
                            }

                            return top10Race;
                        }
                    }
                }
                catch
                {
                    string DBError = "Database Error";
                    return DBError;
                }
            }
        }

        public string GetTop10Time()
        {
            string query = "SELECT nombre_jugador, nombre_carrera, tiempo FROM vista_tiempos ORDER BY tiempo ASC LIMIT 10";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            string top10Time = "Time\n\n";

                            while (reader.Read())
                            {
                                float tiempo = reader.GetFloat("tiempo");

                                top10Time += tiempo + "\n";
                            }

                            return top10Time;
                        }
                    }
                }
                catch
                {
                    string DBError = "Database Error";
                    return DBError;
                }
            }
        }
    }
}