using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpController : MonoBehaviour {
    IPEndPoint EP;
    UdpClient receiver;
    public float x1;
    public float y1;
    public float x2;
    public float y2;
    void Start()
    {
        receiver = new UdpClient(9003, AddressFamily.InterNetwork);
        EP = new IPEndPoint(IPAddress.Any, 0);
    }

    void Update () {
        byte[] data = receiver.Receive(ref EP);
        byte[] x = new byte[4];
        byte[] y = new byte[4];
        Array.Copy(data, 0, x, 0, 4);
        Array.Copy(data, 4, y, 0, 4);
        x1 = BitConverter.ToInt32(x, 0) / 100;
        y1 = BitConverter.ToInt32(y, 0) / 100;
    }
}
