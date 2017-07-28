using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {
    [SerializeField]
    GameObject[] Tanks = new GameObject[2];
    [SerializeField]
    float angle = 0;
    public float currentAngle = 0;

	void Update () {

		if(Vector3.Distance(transform.position, Tanks[1].transform.position) > Vector3.Distance(transform.position, Tanks[0].transform.position))
        {
            Vector3 dist = transform.position - Tanks[0].transform.position;
            dist.Normalize();

            Vector3 dir = Tanks[0].transform.position - transform.position;
            Ray ray = new Ray(transform.position, new Vector3(dir.x, dir.y, 0));
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if(hit.collider.gameObject.tag == "Tank")
            {
                float z = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg + 90;
                angle = z;
            }
        }
        if(Vector3.Distance(transform.position, Tanks[1].transform.position) < Vector3.Distance(transform.position, Tanks[0].transform.position))
        {
            Vector3 dist = transform.position - Tanks[1].transform.position;
            dist.Normalize();

            Vector3 dir = Tanks[1].transform.position - transform.position;
            Ray ray = new Ray(transform.position, new Vector3(dir.x, dir.y, 0));
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            if (hit.collider.gameObject.tag == "Tank")
            {
                float z = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg + 90;
                angle = z;
            }
        }

        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
