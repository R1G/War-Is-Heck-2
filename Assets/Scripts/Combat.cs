using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject attackObj;
    public GameObject grenadeObj;
    public AudioSource weaponAudio;
    Health health;
    Animator anim;
    public float attackSpeed;
    public float attackRange;


    bool attackReady = true;
    bool grenadeReady = true;
    bool blockReady = true;
    bool isPlayer = false;

    void Start() {
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        if(gameObject.tag=="Player") {
            isPlayer=true;
        }

    }
    private void Update()
    {
        if(isPlayer && attackReady && Input.GetButtonUp("Fire1"))
        {
            Attack();
            Invoke("SwingSoundEffect", 0.2f);
        } else if(isPlayer && blockReady && Input.GetButtonDown("Fire2")) {
            Block();
        }

        if(isPlayer && grenadeReady && Input.GetKeyUp(KeyCode.G)) {
            ThrowGrenade();
            Invoke("ResetGrenadeTimer", 3f);
        }
    }

    private void ResetAttackTimer() {
        attackReady = true;
    }

    private void ResetGrenadeTimer() {
        grenadeReady = true;
    }

    public void Attack() {
        anim.SetTrigger("Hit");
        Invoke("LightAttack", 0.5f);
    }

    public void Block() {
        anim.SetTrigger("Block");
    }

    private void LightAttack() {
        GameObject attack = Instantiate(attackObj, transform.position+transform.forward, Quaternion.identity);
        attack.GetComponent<Attack>().attacker = this.gameObject;
        attackReady = false;
        Invoke("ResetAttackTimer", attackSpeed);
    }

    private void ThrowGrenade() {
        GameObject grenade = Instantiate(grenadeObj, transform.position+Vector3.up*2, Quaternion.identity);
        grenade.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*60f);
        grenadeReady=false;
    }

    private void ShieldBlock() {
        health.isImmune=true;
        blockReady=false;
        Invoke("Unblock", 0.5f);
    }

    private void Unblock() {
        health.isImmune=false;
        blockReady=true;
    }

    private void SwingSoundEffect() {
        weaponAudio.Play();
    }


}

