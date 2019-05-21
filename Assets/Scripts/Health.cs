using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.ThirdPerson;

public class Health : MonoBehaviour
{
    public int startingHealth;
    public bool non_aberrating;
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
        startingColor = mesh.material.GetColor("_EmissionColor");
        ResetHealth();
        if(!non_aberrating)
            AberrateScale();
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
        Color prevColor = mesh.material.GetColor("_EmissionColor");
        float health_ratio = (float)damage/(float)(health+damage);
        Debug.Log(health_ratio);
        mesh.material.SetColor("_EmissionColor", prevColor+Color.gray*health_ratio);
    }

    private void RestoreColor() {
        mesh.material.SetColor("_EmissionColor", startingColor);
    }

    private void Die() {
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

    void RemoveImmunity() {
        isImmune=false;
    }

    void AberrateScale() {
        float scaleOffset = Random.Range(0.8f, 1.2f);
        
        UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter tp = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
        tp.m_MoveSpeedMultiplier *= Mathf.Pow(1/scaleOffset, 4);
        transform.localScale *= scaleOffset;
    }
}
