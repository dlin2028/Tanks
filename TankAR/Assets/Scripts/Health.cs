using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float CurrentHealth = 100;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Bullet>())
        {
            CurrentHealth -= collision.gameObject.GetComponent<Bullet>().Damage;
        }
    }


}
