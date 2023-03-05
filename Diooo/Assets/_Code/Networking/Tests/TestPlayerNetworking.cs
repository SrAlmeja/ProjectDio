using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestPlayerNetworking : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private NetworkVariable<int> _randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    private NetworkVariable<MyCustomData> _myCustomData = new NetworkVariable<MyCustomData>(
        new MyCustomData
        {
            MyInt = 1, MyFloat = 2, MyBool = true
        }, 
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        _randomNumber.OnValueChanged += (prevValue, newValue) =>
        {
            Debug.Log("CLIENT = " + OwnerClientId + " RANDOM NUM =  " + _randomNumber.Value);
        };
        
        _myCustomData.OnValueChanged += (prevValue, newValue) =>
        {
            Debug.Log("CLIENT = " + OwnerClientId + " MY CUSTOM DATA =  " + newValue.MyInt+ "; " + newValue.MyFloat + "; " + newValue.MyBool);
        };
    }

    void Update()
    {
        if(!IsOwner)
        {
            return;
        }
    
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _randomNumber.Value = Random.Range(1, 10);
            
            _myCustomData.Value = new MyCustomData
            {
                MyInt = Random.Range(1, 10),
                MyFloat = Random.Range(1, 10),
                MyBool = Random.Range(0, 2) == 0
            };
            
            
        }

        
        
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}


public struct MyCustomData : INetworkSerializable
{
    public int MyInt;
    public float MyFloat;
    public bool MyBool;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref MyInt);
        serializer.SerializeValue(ref MyFloat);
        serializer.SerializeValue(ref MyBool);
    }
}