using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject attackObj;
    Health health;
    Animator anim;
    public float attackSpeed;
    public float attackRange;
    bool attackReady = true;
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
        if(isPlayer && attackReady && Input.GetButtonDown("Fire1"))
        {
            Attack();
        } else if(isPlayer && blockReady && Input.GetButtonDown("Fire2")) {
            Block();
        }
    }

    private void ResetAttackTimer() {
        attackReady = true;
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

    private void ShieldBlock() {
        health.isImmune=true;
        blockReady=false;
        Invoke("Unblock", 0.5f);
    }

    private void Unblock() {
        health.isImmune=false;
        blockReady=true;
    }


}

