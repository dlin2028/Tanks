using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    UdpController Udp;

    private void Start()
    {
        Udp = GameObject.FindGameObjectWithTag("GameController").GetComponent<UdpController>();   
    }

    void Update () {
        transform.position = new Vector2(Udp.x1, Udp.x2);
        transform.rotation = Quaternion.Euler(0, 0, Udp.angle);
	}
}
