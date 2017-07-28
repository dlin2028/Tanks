using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    public KeyCode key;
    public GameObject bullet;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(key))
        {
            GameObject createdObject = Instantiate(bullet);
            createdObject.transform.position = transform.position;
            createdObject.transform.rotation = transform.rotation;
        }
	}
}
