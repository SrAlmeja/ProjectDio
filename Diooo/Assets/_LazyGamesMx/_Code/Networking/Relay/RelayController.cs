//Dino/05/04/2023 Creation of the class this class will manage the connection between players in different computers
// this script is the connection to the relay server

using System;
using System.Collections;
using System.Collections.Generic;
using QFSW.QC;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class RelayController : MonoBehaviour
    {
        #region public variables

        public static RelayController Instance;

        public string RelayJoinCode => _relayJoinCode;
        

        #endregion
        
        #region private variables
        //Do not include the host in the max connections
        [SerializeField] private int maxConnections = 3;
        
        string _relayJoinCode = "";
        #endregion



        #region Unity Methods

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {

        }

        void Update()
        {

        }
        #endregion
        
        [Command]
        #region public methods
        public async void CreateRelayServer(string joinedLobbyId, string KEY_RELAY_JOIN_CODE)
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
                 _relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log("<color=#EE92FF>Join RELAY code: </color>" + _relayJoinCode);

                await LobbyService.Instance.UpdateLobbyAsync(joinedLobbyId, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject> {
                        { KEY_RELAY_JOIN_CODE , new DataObject(DataObject.VisibilityOptions.Member, _relayJoinCode) }
                    }
                });

                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
                DioGameMultiplayer.Instance.StartHost();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public async void JoinRelayServer(string joinCode)
        {
            try
            {
                Debug.Log("<color=#EE92FF>Joining relay server...</color>" +  joinCode);
                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));
                DioGameMultiplayer.Instance.StartClient();
            }
            catch (RelayServiceException e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
        
        
        #endregion

    }
}