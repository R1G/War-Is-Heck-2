using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behavior : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    public Material blueMaterial;
    NavMeshAgent agent;
    enum npcState {Idle, Siege, Combat};
    npcState state = npcState.Idle;
    Combat combat;
    GameObject enemyBase;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        combat=GetComponent<Combat>();
        agent=GetComponent<NavMeshAgent>();
        if(gameObject.tag=="BLUE") {
            mesh.material=blueMaterial;
            enemyBase=GameObject.Find("RedBase");
        } else {
            enemyBase=GameObject.Find("BlueBase");
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) {
            case(npcState.Idle): {
                if(enemyBase!=null) {
                    state = npcState.Siege;
                }
                break;
            }
            case(npcState.Siege): {
                target=GetEnemyInLoS();
                if(enemyBase==null) {
                    state=npcState.Idle;
                    break;
                }
                if(target!=null) {
                    state=npcState.Combat;
                } else {
                    agent.SetDestination(enemyBase.transform.position);
                    if(Vector3.Distance(enemyBase.transform.position,transform.position)<=combat.attackRange+1.5f) {
                        combat.Attack();
                    }
                }
                break;
            }
            case(npcState.Combat): {
                if(target==null) {
                    state=npcState.Idle;
                    break;
                } 
                if(Vector3.Distance(gameObject.transform.position, target.transform.position)<=combat.attackRange) {
                    combat.Attack();
                } 
                agent.SetDestination(target.transform.position);
                break;
            }
        }
    }

    GameObject GetEnemyInLoS() {
        if(gameObject.tag=="BLUE") {
            return GameObject.FindGameObjectWithTag("RED");
        } else {
            return GameObject.FindGameObjectWithTag("BLUE");
        }
    }
}
