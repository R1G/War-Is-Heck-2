using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public GameObject attacker; //Don't want to damage attacker
    public float attackDuration;
    public float velocity;

    public GameObject graphic;
    Rigidbody rb;
    
    private void Start()
    {
        Invoke("EndAttack", attackDuration);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward*velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject!=attacker) {
            other.gameObject.SendMessageUpwards("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            if(graphic!=null) {
                Instantiate(graphic, transform.position, Quaternion.identity);
            }   
            Invoke("EndAttack", 0.2f);
        }
    }

    void EndAttack()
    {
        Destroy(gameObject);
    }
}
