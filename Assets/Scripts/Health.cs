using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.ThirdPerson;

public class Health : MonoBehaviour
{
    public delegate void KillUnit(GameObject go); public static event KillUnit OnKillUnit;
    public int startingHealth;
    GameManager gm;
    public GameObject corpse;
    SkinnedMeshRenderer mesh;
    int health;
    bool isDead = false;
    public bool isImmune = false;
    private Color startingColor;

    private void Start() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        mesh=GetComponentInChildren<SkinnedMeshRenderer>();
        ResetHealth();
    }

    private void ResetHealth() {
        health = (startingHealth>0)? startingHealth : health;
        isDead=false;
        isImmune=false;
    }

    private void TakeDamage(int damage) {
        if(isDead || isImmune) {
            return;
        }
        health -= damage;
        
        isImmune = true;
        if(health<=0) {
            RestoreColor();
            Die();
        } else {
            AdjustColor(damage);
        }
        Invoke("RemoveImmunity", 0.3f);
    }

    private void AdjustColor(int damage) {
        if(mesh==null) {return;}
        Color prevColor = mesh.material.GetColor("_EmissionColor");
        float health_ratio = (float)damage/(float)(health+damage);
        mesh.material.SetColor("_EmissionColor", prevColor+Color.gray*health_ratio);
    }

    private void RestoreColor() {
        mesh.material.SetColor("_EmissionColor", startingColor);
    }

    private void Die() {
        if(isDead) 
            return;
        isDead=true;
        OnKillUnit?.Invoke(gameObject);
        Instantiate(corpse, transform.position+Vector3.up, Quaternion.identity);
        Destroy(gameObject);
    }

    void RemoveImmunity() {
        isImmune=false;
    }
}
