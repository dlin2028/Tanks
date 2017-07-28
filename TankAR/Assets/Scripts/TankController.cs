using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {
    UdpController Udp;
    float x = 0;
    float y = 0;
    public float offsetX = 1;
    public float offsetY = 1;

    private void Start()
    {
        Udp = GameObject.FindGameObjectWithTag("GameController").GetComponent<UdpController>();   
    }

    void Update () {
        x = Mathf.Lerp(-6, 5, Mathf.InverseLerp(150, 1171, Udp.x1)) + offsetX;
        y = Mathf.Lerp(-3, 2, Mathf.InverseLerp(654, 160, Udp.y1)) + offsetY;
        transform.position = new Vector2(x, y);
        transform.rotation = Quaternion.Euler(0, 0, Udp.angle);
	}
}
