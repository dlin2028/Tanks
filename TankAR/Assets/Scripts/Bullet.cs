using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int Damage = 20;

    private void OnDestroy()
    {
        Transform child = transform.GetChild(0);
        child.GetComponent<Light>().enabled = true;
        child.GetComponent<Light>().intensity = 10;
        child.GetComponent<Destroyer>().enabled = true;
        child.GetComponent<Destroyer>().AtSeconds = 0.05f;
        transform.DetachChildren();
    }


}
