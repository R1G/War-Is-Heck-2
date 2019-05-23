using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager.Side side;
    public Image timer;
    public float spawnRate;
    public int spawnSize;
    public bool isSpawning = true;

    GameObject spawnObject;
    GameManager gm;
    string spawnTag;

    void Start() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(side==GameManager.Side.Blue) {
            gm.blueBase = gameObject;
            spawnTag = "BLUE";
        } else if(side==GameManager.Side.Red) {
            gm.redBase = gameObject;
            spawnTag = "RED";
        }
        spawnObject = Resources.Load("Meeb") as GameObject;
        timer.fillAmount = 1f;
        InvokeRepeating("ReduceTimerFill", 0f, spawnRate/100f);
    }


    void Update()
    {
        if(isSpawning && (gm.blueCount+gm.redCount)<=30) {
            timer.fillAmount = 1f;
            SpawnSquad();
        }  
    }

    private void ReduceTimerFill() {
        timer.fillAmount -= 0.01f;
    }

    void SpawnSquad() {
        for(int i=0; i<spawnSize; i++) {
            Invoke("Spawn", 0.2f*i);
        }
        isSpawning=false;
        Invoke("ResetSpawn", spawnRate);
        gm.UpdateSquads(side);
    }

    void Spawn() {
        GameObject newMeeb = Instantiate(spawnObject, transform.position+transform.forward, Quaternion.identity) as GameObject;
        newMeeb.tag = spawnTag;
        List<GameObject> squad = (side==GameManager.Side.Blue) ? gm.currentBlueSquad : gm.currentRedSquad;
        newMeeb.GetComponent<NPC_Behavior>().squadIndex = gm.GetSquadIndex(side)-1;
        newMeeb.GetComponent<Combat>().weapon = PickRandomWeapon();
    }

    void ResetSpawn() {
        isSpawning=true;
    }

    GameManager.Weapon PickRandomWeapon() {
        float rand = Random.Range(0f, 2f);
        return rand<=0.2f ? GameManager.Weapon.PillGun : GameManager.Weapon.Machete;
    }
}
