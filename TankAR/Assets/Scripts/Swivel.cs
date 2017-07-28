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
                transform.rotation = Quaternion.Euler(0, 0, controller.swivel1 + 90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, controller.swivel1 + 90) ;
            }
        }
        else
        {
            if (controller.swivel2 < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, controller.swivel2 - 90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, controller.swivel2 - 90);
            }
        }
    }
}
