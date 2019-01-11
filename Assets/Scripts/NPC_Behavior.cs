using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behavior : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    GameManager gm;
    public Material blueMaterial;
    NavMeshAgent agent; 
    Combat combat;

    public float maxViewRange;
    
    enum npcState {Idle, Siege, Combat};
    npcState state = npcState.Idle;

    GameObject enemyBase;
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        combat=GetComponent<Combat>();
        agent=GetComponent<NavMeshAgent>();
        if(gameObject.tag=="BLUE") {
            mesh.material=blueMaterial;
            enemyBase=GameObject.Find("RedBase");
            gm.blueTeam.Add(this.gameObject);
            gm.blueCount++;
        } else {
            enemyBase=GameObject.Find("BlueBase");
            gm.redTeam.Add(this.gameObject);
            gm.redCount++;
            
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
                if(enemyBase==null) {
                    state=npcState.Idle;
                    break;
                }
                target=GetEnemyInLoS();

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
                    transform.LookAt(target.transform.position);
                    combat.Attack();
                } 
                agent.SetDestination(target.transform.position);
                break;
            }
        }
    }

    GameObject GetEnemyInLoS() {
        List<GameObject> enemies;
        if(gameObject.tag=="BLUE") {
            enemies=gm.redTeam;
        } else {
            enemies=gm.blueTeam;
            int randomIndex = Random.Range(0, enemies.Count-1);
            enemies.Insert(randomIndex, gm.player);
        }
        foreach(GameObject enemy in enemies) {
            if(enemy==null) {
                continue;
            }
            if(isInLoS(enemy)) {
                return enemy;
            }
        }
        return null;
    }

    bool isInLoS(GameObject losTarget) {
        if(GetDistanceFrom(losTarget)>maxViewRange) {
            return false;
        }
        return true;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, GetDirectionTo(losTarget), out hit, maxViewRange-1f)) {
            if(hit.transform.gameObject==losTarget) {
                return true;
            }
        }
        return false;
        
    }

    float GetDistanceFrom(GameObject distTarget) {
        if(distTarget==null) {
            return Mathf.Infinity;
        }
        return Vector3.Distance(transform.position, distTarget.transform.position);
    }

    Vector3 GetDirectionTo(GameObject dirTarget) {
        return dirTarget.transform.position-transform.position;
    }
}
