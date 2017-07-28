using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    [SerializeField]
    GameObject[] Tanks = new GameObject[2];
	void Start () {
		
	}

	void Update () {
		if(Vector3.Distance(transform.position, Tanks[0].transform.position) > Vector3.Distance(transform.position, Tanks[1].transform.position))
        {

        }
        else
        {

        }
	}
}
