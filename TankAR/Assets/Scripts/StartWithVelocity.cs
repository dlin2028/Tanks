using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StartWithVelocity : MonoBehaviour {

    public Vector3 velocity;
	void Start () {
        GetComponent<Rigidbody>().AddForce(velocity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
