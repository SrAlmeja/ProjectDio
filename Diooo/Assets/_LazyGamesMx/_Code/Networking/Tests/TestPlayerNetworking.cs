using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class TestPlayerNetworking : NetworkBehaviour
{
    #region Spawn Objects Prefab
    
    //Referencia del prefab del objeto a instanciar
    [SerializeField] private Transform objectPrefab;
    //Referencia del objeto en escena
    private Transform spawnedTransform;
    #endregion

    
    #region Networked Variables
    
    //Variables que existen en la red
    //Se le otorgan los permisos de lectura y escritura
    private NetworkVariable<int> _randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    //Puedes mandar constructores a la red de tipo VALUE Pero NOOO REFERENCES
    private NetworkVariable<MyCustomData> _myCustomData = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            MyInt = 1, MyFloat = 2, MyBool = true
        }, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    #endregion
    
    #region Methods for the NetworkBehaviour
    // Este es el "Start()" del player, ya que los objetos se instancian 
    // Aquí se suscriben los métodos a los eventos de la red
    public override void OnNetworkSpawn()
    {
        _randomNumber.OnValueChanged += (prevValue, newValue) =>
        {
            Debug.Log("CLIENT = " + OwnerClientId + " RANDOM NUM =  " + _randomNumber.Value);
        };
        
        _myCustomData.OnValueChanged += (prevValue, newValue) =>
        {
            Debug.Log("CLIENT = " + OwnerClientId + " MY CUSTOM DATA =  " + newValue.MyInt+ "; " + newValue.MyFloat + "; " + newValue.MyBool + "; " + newValue.MyString);
        };
    }
    #endregion

    void Update()
    {
        #region Owner
        // Saber si el objeto es el del cliente
        // Aquí evitas que se ejecute código en el cliente que no le corresponde
        // Por ejemplo, si el host mueve su player, no se ejecuta el código de movimiento en otro cliente
        if(!IsOwner)
        {
            return;
        }
        #endregion

        if(Input.GetKeyDown(KeyCode.Space))
        {

            // #region Spawned Object Network
            //
            // spawnedTransform = Instantiate(objectPrefab);
            // spawnedTransform.GetComponent<NetworkObject>().Spawn(true);
            //
            // #endregion
            //
            // #region Despawning Networked Object
            //
            // // Despawnea el objeto en la red pero no destruye el objeto
            // spawnedTransform.GetComponent<NetworkObject>().Despawn(true);
            // Destroy(spawnedTransform.gameObject);
            //
            //
            // #endregion
            //
            //
            // #region Servers RPC TEST
            //
            // TestServerRpc("Hello server");
            // ParamsServerRpc(new ServerRpcParams());
            //
            // TestClientRpc(new ClientRpcParams(
            //     // {
            //     //     Send = new ClientRpcSendParams {TargetClientIds = new List<ulong>{1}}
            //     // }
            // ));
            //
            // #endregion
            
            #region Values Test
            _randomNumber.Value = Random.Range(1, 10);
            _myCustomData.Value = new MyCustomData
            {
                MyInt = Random.Range(1, 10),
                MyFloat = Random.Range(1, 10),
                MyBool = true,
                MyString = "Hello World"
            };
            #endregion

        }

        #region Plyer Movement
        float moveSpeed = 5f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        #endregion

    }

    #region Server RPCs
    
    //SI O SI  DEBE TERMINAR EL NOMBRE DEL METODO CON RPC
    // Este codigo no corre en el cliente, corre en el servidor
    // El servidor puede ser un "Host" o un "Server"
    // Si el cliente manda este método, el servidor lo ejecuta, pero no el cliente
    // o sea, el cliente manda el mensaje y el servidor lo ejecuta
    // Aaquí si se pueden usar strings pero no referencias, sólo valores primitivos
    
    //Client => Server
    [ServerRpc]
    private void TestServerRpc(string message)
    {
        Debug.Log(" Test Server RPC ==" + OwnerClientId + " " + message);
    }
    //Otra forma de que el cliente mande un mensaje al servidor, ejemplo su ID
    [ServerRpc]
    private void ParamsServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log(" Test Server RPC ==" + OwnerClientId + " " + serverRpcParams.Receive.SenderClientId);
    }
    
    
    // Este codigo lo manda el server al cliente y puedes designar a qué cliente mandar mediante el ID
    // Server => Client
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log(" Test Client RPC ==" + OwnerClientId + " ");
    }
    #endregion
    
}


public struct MyCustomData : INetworkSerializable
{
    public int MyInt;
    public float MyFloat;
    public bool MyBool;
    public FixedString128Bytes MyString;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref MyInt);
        serializer.SerializeValue(ref MyFloat);
        serializer.SerializeValue(ref MyBool);
        serializer.SerializeValue(ref MyString);
    }
}