using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LobbyController.Instance.OnPlayerEnterRoom += JoinPlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void JoinPlayer()
    {
        
    }

}
