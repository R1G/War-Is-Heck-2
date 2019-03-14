using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class Health : MonoBehaviour
{
    public int startingHealth;
    GameManager gm;
    public GameObject corpse;
    GameObject healthMesh;
    TextMesh healthText;
    int health;
    bool isDead = false;
    public bool isImmune = false;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        healthMesh = Instantiate(Resources.Load("HealthMesh"), transform.position, Quaternion.identity) as GameObject;
        healthMesh.GetComponent<FollowTarget>().target=gameObject.transform;
        healthMesh.GetComponent<FollowTarget>().offset= new Vector3(0f, 1.5f, 0f);
        healthText = healthMesh.GetComponent<TextMesh>();
        
        if(startingHealth>1000) {
            healthText.characterSize=0.2f;
        }

        if(startingHealth>10000) {
            healthText.characterSize=0.4f;
        }
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
        healthText.text=health.ToString();
        isImmune=false;
    }

    void TakeDamage(int damage)
    {
        if(isDead || isImmune) {
            return;
        }
        health -= damage;
        isImmune = true;
        healthText.text=health.ToString();
        if(health<=0)
        {
            Die();
        }
        Invoke("RemoveImmunity", 0.3f);
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
        Destroy(healthMesh);
        Destroy(gameObject);
    }

    void RemoveImmunity() {
        isImmune=false;
    }
}
