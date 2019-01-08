using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startingHealth;
    public GameObject corpse;
    int health;

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
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if(health<=0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Dead!");
        Instantiate(corpse, transform.position+Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }
}
