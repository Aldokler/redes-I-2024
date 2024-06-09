using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button Btn_Server;
    [SerializeField] private Button Btn_Host;
    [SerializeField] private Button Btn_Client;

    private void Awake()
    {
        //Lamda expression / Delegate
        Btn_Server.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });

        //Lamda expression / Delegate
        Btn_Host.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });

        //Lamda expression / Delegate
        Btn_Client.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}