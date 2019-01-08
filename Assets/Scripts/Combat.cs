using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject attackObj;
    Animator anim;
    public float attackSpeed;
    public float attackRange;
    bool attackReady = true;
    bool isPlayer = false;

    void Start() {
        anim=GetComponent<Animator>();
        if(gameObject.tag=="Player") {
            isPlayer=true;
        }

    }
    private void Update()
    {
        if(isPlayer && attackReady && Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void ResetAttackTimer() {
        attackReady = true;
    }

    public void Attack() {
        anim.SetTrigger("Hit");
        Invoke("LightAttack", 0.5f);
    }

    private void LightAttack() {
        GameObject attack = Instantiate(attackObj, transform.position+transform.forward, Quaternion.identity);
        attack.GetComponent<Attack>().attacker = this.gameObject;
        attackReady = false;
        Invoke("ResetAttackTimer", attackSpeed);
    }


}

