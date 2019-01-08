using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startingHealth;
    public GameObject corpse;
    int health;
    bool isDead = false;

    void Start()
    {
        ResetHealth();
    }

    void ResetHealth() 
    {
        if (startingHealth > 0)
        {
            health = startingHealth;
        }
        else
        {
            health = 100;
        }
        isDead=false;
    }

    void TakeDamage(int damage)
    {
        if(isDead) {
            //return;
        }
        health -= damage;
        if(health<=0)
        {
            Die();
        }
    }

    void Die()
    {
        if(isDead) {
            return;
        }
        isDead=true;
        if(gameObject.tag=="Player") {
            GameManager.KillPlayer();
        } else if(gameObject.tag=="RED") {
            GameManager.KillRed();
        } else if(gameObject.name=="RedBase") {
            GameManager.DestroyRedBase();
        } else if(gameObject.name=="BlueBase") {
            GameManager.DestroyBlueBase();
        }

        Instantiate(corpse, transform.position+Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }
}
