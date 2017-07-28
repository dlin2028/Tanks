using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    [SerializeField]
    GameObject[] Tanks = new GameObject[2];
	void Start () {
		
	}

	void Update () {
		if(Vector3.Distance(transform.position, Tanks[1].transform.position) > Vector3.Distance(transform.position, Tanks[0].transform.position))
        {
            Vector3 dist = transform.position - Tanks[0].transform.position;
            dist.Normalize();

            float z = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, z - 90);
        }
        else
        {
            Vector3 dist = transform.position - Tanks[1].transform.position;
            dist.Normalize();

            float z = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, z - 90);
        }
	}
}
