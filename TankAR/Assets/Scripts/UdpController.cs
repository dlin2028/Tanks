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
    public float angle;
    public float swivel1;
    public float swivel2;
    void Start()
    {
        receiver = new UdpClient(9003, AddressFamily.InterNetwork);
        EP = new IPEndPoint(IPAddress.Any, 0);
    }

    void Update () {
        byte[] data = receiver.Receive(ref EP);
        byte[] x = new byte[4];
        byte[] y = new byte[4];
        byte[] a = new byte[4];
        byte[] s1 = new byte[4];
        byte[] s2 = new byte[4];

        Array.Copy(data, 0, x, 0, 4);
        Array.Copy(data, 4, y, 0, 4);
        Array.Copy(data, 8, a, 0, 4);
        Array.Copy(data, 12, s1, 0, 4);
        Array.Copy(data, 16, s2, 0, 4);
        x1 = BitConverter.ToInt32(x, 0) / 100f;
        y1 = BitConverter.ToInt32(y, 0) / 100f;
        angle = BitConverter.ToInt32(a, 0) + 180 - 90;
        angle = 360 - angle;

        swivel1 = interpSwivel(BitConverter.ToInt32(s1, 0));
        swivel2 = interpSwivel(BitConverter.ToInt32(s2, 0));
    }

    private float interpSwivel(float angle)
    {
        if (angle < 0)
        {
            return Mathf.Lerp(0, 90, (angle + 90) / -90);
        }
        else
        {
            return Mathf.Lerp(180, 90, (angle - 90) / 90);
        }
    }
}
