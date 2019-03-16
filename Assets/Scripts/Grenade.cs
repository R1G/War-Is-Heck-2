using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float velocity;
    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward*velocity;
    }
    void OnCollisionEnter() {
        Invoke("Explode", 0.2f);
    }

    void Explode() {
        Destroy(gameObject);
    }
}
