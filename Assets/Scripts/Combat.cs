using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Combat : MonoBehaviour
{
    public GameObject attackObj;
    public GameObject grenadeObj;
    Animator anim;
    NavMeshAgent agent;
    NPC_Behavior behavior;
    public float attackSpeed;
    public float attackRange;

    bool attackReady = true;
    bool grenadeReady = true;
    bool isPlayer = false;

    void Start() {
        anim = GetComponent<Animator>();

        if(gameObject.tag=="Player") {
            isPlayer=true;
        } else {
            agent = GetComponent<NavMeshAgent>();
            behavior = GetComponent<NPC_Behavior>();
        }

    }
    private void Update() {
        if(isPlayer && attackReady && Input.GetButtonUp("Fire1")) {
            Invoke("Attack", attackSpeed);
            anim.SetTrigger("Hit");
            attackReady = false;
        } else if(isPlayer && grenadeReady && Input.GetKeyUp(KeyCode.G)) {
            ThrowGrenade();
        }
    }

    public void Attack() {
        if(isPlayer) {
            GameObject attack = Instantiate(attackObj, transform.position+transform.forward, Quaternion.identity);
            attack.GetComponent<Attack>().attacker = this.gameObject;
            Invoke("ResetAttackTimer", attackSpeed);
            return;
        }
        if(behavior.target != null) {
            agent.SetDestination(behavior.target.transform.position);
            if(attackReady && Vector3.Distance(transform.position, behavior.target.transform.position)<=attackRange) {
                GameObject attack = Instantiate(attackObj, transform.position+transform.forward, Quaternion.identity);
                attack.GetComponent<Attack>().attacker = this.gameObject;
                attackReady=false;
                anim.SetTrigger("Hit");
                Invoke("ResetAttackTimer", attackSpeed);
            }
        }

    }

    public void AttackBase() {
        if(isPlayer) {
            return;
        }

        if(behavior.enemyBase!=null) {
            agent.SetDestination(behavior.enemyBase.transform.position);
            if(attackReady && Vector3.Distance(transform.position, behavior.enemyBase.transform.position)<=attackRange) {
                GameObject attack = Instantiate(attackObj, transform.position+transform.forward, Quaternion.identity);
                attack.GetComponent<Attack>().attacker = this.gameObject;
                attackReady=false;
                anim.SetTrigger("Hit");
                Invoke("ResetAttackTimer", attackSpeed);
            }
        }
    }

    private void ThrowGrenade() {
        grenadeReady=false;
        GameObject grenade = Instantiate(grenadeObj, transform.position+Vector3.up*2, Quaternion.identity);
        grenade.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*200f);
        grenade.transform.rotation = Camera.main.transform.rotation;
        grenade.transform.Rotate(new Vector3(-25f, 90f, 0f));
        Invoke("ResetGrenadeTimer", 1f);
    }

    private void ResetAttackTimer() {
        attackReady = true;
    }

    private void ResetGrenadeTimer() {
        grenadeReady = true;
    }
}

