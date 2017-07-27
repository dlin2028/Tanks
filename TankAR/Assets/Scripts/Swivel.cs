using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swivel : MonoBehaviour {

    enum SwivelState
    {
        Closed,
        Open
    }
    
    public bool isLeftGate; //Temporary variable so we know which way to turn from default position

    int angle = 0;

    void Start ()
    {

	}
	
	void Update ()
    {   
        if (isLeftGate)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * 20);
        }
        else
        {
            transform.Rotate(Vector3.back * Time.deltaTime * -20);
        }   
    }
}
