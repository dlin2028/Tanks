using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swivel : MonoBehaviour {

    enum SwivelState
    {
        Closed,
        Open
    }

    public GameObject Block1;
    public GameObject Block2;

    public BoxCollider Block1Collider;
    public BoxCollider Block2Collider;

    private SwivelState state = SwivelState.Closed;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        //Block1.transform.Rotate(Vector3.back * Time.deltaTime * 10);
        //Block1.transform.RotateAround(new Vector3(Block1Collider.bounds.size.x, 0, 0). )
	}
}
