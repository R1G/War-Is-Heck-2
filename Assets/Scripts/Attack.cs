using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public GameObject attacker; //Don't want to damage attacker
    public float attackDuration;
    private void Start()
    {
        Invoke("EndAttack", attackDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject!=attacker) {
            other.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    void EndAttack()
    {
        Destroy(gameObject);
    }
}
