using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swivel : MonoBehaviour {

    public UdpController controller;
    public int swivelID = 1;

    void Start ()
    {
        
	}
	
	void Update ()
    {
        if(swivelID == 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, controller.swivel1));
        }
        else if(swivelID == 2)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, controller.swivel2));
        }
    }
}
