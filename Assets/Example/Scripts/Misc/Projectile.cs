using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour {
    public float velocity = 10f;
    public float lifeSpan = 2f;

    public void Start() {
        GetComponent<Rigidbody>().velocity = transform.forward * velocity;
        GetComponent<Rigidbody>().useGravity = false;

        Destroy(this.gameObject, lifeSpan);
    }
}
