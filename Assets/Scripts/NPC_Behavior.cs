using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Behavior : MonoBehaviour
{
    SkinnedMeshRenderer mesh;
    GameManager gm;
    Combat combat;
    public int squadIndex;
    List<GameObject> squad;
    enum NPC {Idle, Combat, Siege};
    NPC state = NPC.Idle;
    GameManager.Side side;
    public float maxViewRange;
    public GameObject enemyBase;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        combat=GetComponent<Combat>();
        mesh=GetComponentInChildren<SkinnedMeshRenderer>();
        setTeam();
        squad = gm.GetSquad(side, squadIndex);
    }

    void setTeam() {
        if(gameObject.tag=="BLUE") {
            mesh.material=Resources.Load("emmBlue") as Material;
            enemyBase=GameObject.Find("RedBase");
            gm.blueTeam.Add(this.gameObject);
            gm.blueCount++;
            side = GameManager.Side.Blue;
        } else {
            mesh.material=Resources.Load("emmRed") as Material;
            enemyBase=GameObject.Find("BlueBase");
            gm.redTeam.Add(this.gameObject);
            gm.redCount++;
            side = GameManager.Side.Red;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch(state) {
            case(NPC.Idle): {
                enemyBase = FindEnemyBase();
                target = FindEnemyInLoS();
                if(enemyBase!=null) {state=NPC.Siege;}
                if(target!=null) {state=NPC.Combat;}
                break;
            }
            case(NPC.Combat): {
                if(target!=null) {combat.Attack(target);} else {state = NPC.Idle;}
                break;
            }
            case(NPC.Siege): {
                target = FindEnemyInLoS();
                if(target!=null) {state=NPC.Combat; break;}
                if(enemyBase!=null) {combat.Attack(enemyBase);} else {state=NPC.Idle;};
                break;
            }
        }
    }
    
    GameObject FindEnemyBase() {
        if(gameObject.tag=="BLUE") {
            return gm.redBase;
        } else {
            return gm.blueBase;
        }
    }
    GameObject FindEnemyInLoS() {
        //Set Team
        List<GameObject> enemies;
        if(gameObject.tag=="BLUE") {
            enemies=gm.redTeam;
        } else {
            enemies=gm.blueTeam;
        }
        //Find Enemy
        GameObject foundEnemy = null;
        foreach(GameObject enemy in enemies) {
            if(enemy==null) {
                continue;
            }
            if(isInLoS(enemy)) {
                foundEnemy=enemy;
                break;
            }
        }
        //Supply to squad
        if(foundEnemy!=null && foundEnemy.GetComponent<NPC_Behavior>()!=null) {
            foundEnemy.GetComponent<NPC_Behavior>().target=gameObject;
            foreach(GameObject friendly in squad) {
                if(friendly==null) {
                    continue;
                }
                friendly.GetComponent<NPC_Behavior>().target = foundEnemy;
            }
        }
        
        return foundEnemy;
    }
    bool isInLoS(GameObject losTarget) {
        //Check In Range
        if(GetDistanceFrom(losTarget)>maxViewRange) {
            return false;
        }
        //Check Field of View
        if(Vector3.Angle(GetDirectionTo(losTarget), transform.forward) > 45.0) {
            return false;
        }
        //Check Obstacles
        RaycastHit hit;
        if(Physics.Raycast(transform.position, GetDirectionTo(losTarget), out hit, GetDistanceFrom(losTarget)-2f)) {
            return false;
        }
        return true;
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
