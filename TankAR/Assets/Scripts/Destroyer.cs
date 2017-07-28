using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {

    public float AtSeconds = -1;
    public bool OnCollision = false;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }



    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(AtSeconds >= 0)
        {
            StartCoroutine(destroyAtSeconds());
            AtSeconds = -2;
        }
	}

    IEnumerator destroyAtSeconds()
    {
        yield return new WaitForSeconds(AtSeconds);
        Destroy(gameObject);
    }
}
