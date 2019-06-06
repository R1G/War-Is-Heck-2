using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(Combat))]
public class NPC_Behavior : MonoBehaviour
{
    public delegate void CreateNPC(NPC_Behavior n); public static event CreateNPC OnCreateNPC;
    SkinnedMeshRenderer mesh;
    GameManager gm;
    Combat combat;
    List<GameObject> squad;
    enum NPC {Idle, Combat, Siege};
    NPC state = NPC.Idle;
    public GameManager.Side side;
    public float maxViewRange;
    public GameObject enemyBase;
    public GameObject target;

    // Start is called before the first frame update
    void Start() {
        combat=GetComponent<Combat>();
        mesh=GetComponentInChildren<SkinnedMeshRenderer>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        OnCreateNPC?.Invoke(this);
        setTeam();
        squad = gm.GetSquad(side);
    }

    void setTeam() {
        if(side==GameManager.Side.Blue) {
            enemyBase=GameObject.Find("RedBase");
        } else {
            enemyBase=GameObject.Find("BlueBase");
        }
    }
    // Update is called once per frame
    void Update() {
        switch(state) {
            case(NPC.Idle): {
                enemyBase = gm.GetEnemySpawner(side);
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
    
    GameObject FindEnemyInLoS() {
        //Set Team
        List<GameObject> enemies = (side==GameManager.Side.Red) ? gm.GetTeam(GameManager.Side.Blue) : gm.GetTeam(GameManager.Side.Red);
        //Find Enemy
        GameObject foundEnemy = null;
        foreach(GameObject enemy in enemies) {
            if(enemy==null) {
                continue;
            }
            if(Utils.IsInLoS(gameObject, enemy)) {
                foundEnemy=enemy;
                break;
            }
        }
        //Supply to squad
        if(foundEnemy!=null && foundEnemy.GetComponent<NPC_Behavior>()!=null) {
            foundEnemy.GetComponent<NPC_Behavior>().target=gameObject;
            foreach(GameObject friendly in squad) {
                if(friendly!=null)
                    friendly.GetComponent<NPC_Behavior>().target = foundEnemy;
            }
        }
        return foundEnemy;
    }
}
