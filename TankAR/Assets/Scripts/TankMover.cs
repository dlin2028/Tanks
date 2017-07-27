using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : MonoBehaviour {

    public GameObject Map;
    public int tankID;
    private Vector2 position;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(
            Mathf.Lerp(Map.transform.position.x, Map.transform.position.x + Map.GetComponent<BoxCollider>().size.x, position.x/1280),
            Mathf.Lerp(Map.transform.position.x, Map.transform.position.x + Map.GetComponent<BoxCollider>().size.x, position.y/720),
            transform.position.z);
    }
}
