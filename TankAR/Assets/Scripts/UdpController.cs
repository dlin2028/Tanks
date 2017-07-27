using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpController : MonoBehaviour {
    IPEndPoint EP;
    UdpClient receiver;
    void Start()
    {
        receiver = new UdpClient(9003, AddressFamily.InterNetwork);
        EP = new IPEndPoint(IPAddress.Any, 0);
    }

    void Update () {
        byte[] received = receiver.Receive(ref EP);
        for(int i = 0; i < received.Length; i++)
        {
            Debug.Log(received[i]);
        }
    }
}
