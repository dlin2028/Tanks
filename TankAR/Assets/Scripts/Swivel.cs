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
        if(controller == null)
        {
            return;
        }

        if(swivelID == 1)
        {
            if(controller.swivel1 < 0)
            {
                transform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(10, 178, Mathf.InverseLerp(-95, -180, controller.swivel1) * 0.5f));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-90, -180, 0.5f + Mathf.InverseLerp(180, 92, controller.swivel1) * 0.5f));
            }
        }
        else
        {
            if (controller.swivel2 < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(90, 180, Mathf.InverseLerp(-180, -90, controller.swivel2) * 0.5f));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(90, -10, 0.5f + Mathf.InverseLerp(180, 90, controller.swivel2) * 0.5f));
            }
        }
    }
}
