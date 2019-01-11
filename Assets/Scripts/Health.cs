using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startingHealth;
    GameManager gm;
    public GameObject corpse;
    int health;
    bool isDead = false;
    public bool isImmune = false;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        if(isDead || isImmune) {
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
            gm.KillPlayer();
        } else if(gameObject.tag=="BLUE") {
            gm.KillBlue(gameObject);
        } else if(gameObject.tag=="RED") {
            gm.KillRed(gameObject);
        } else if(gameObject.name=="RedBase") {
            gm.DestroyRedBase();
        } else if(gameObject.name=="BlueBase") {
            gm.DestroyBlueBase();
        }

        Instantiate(corpse, transform.position+Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }
}
