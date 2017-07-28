using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swivel : MonoBehaviour {
    
    int angle = 0;

    void Start ()
    {

	}
	
	void Update ()
    {   
        transform.Rotate(Vector3.back * Time.deltaTime * 20);
    }
}
