using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Net;
using System.Net.Sockets;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button Btn_Server;
    [SerializeField] private Button Btn_Host;
    [SerializeField] private Button Btn_Client;

    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;

    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;

    private void Awake()
    {
        //Lamda expression / Delegate
        Btn_Server.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });

        //Lamda expression / Delegate
        Btn_Host.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            GetLocalIPAddress();
        });

        //Lamda expression / Delegate
        Btn_Client.onClick.AddListener(() => {
            ipAddress = ip.text;
            SetIpAddress();
            NetworkManager.Singleton.StartClient();
        });
    }

    void Start()
    {
        ipAddress = "0.0.0.0";
        SetIpAddress(); // Set the Ip to the above address
    }

    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    public void SetIpAddress()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
    }
}