using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public delegate void SpawnerCreate(Spawner t); public static event SpawnerCreate OnSpawnerCreate;
    public delegate void SquadCreate(GameManager.Side s); public static event SquadCreate OnSquadCreate;
    public GameManager.Side side;
    public Image timer;
    public float spawnRate;
    public int spawnSize;
    public bool isSpawning = true;

    public GameObject spawnObject;
    GameManager gm;
    string spawnTag;

    void Start() {
        OnSpawnerCreate?.Invoke(this);
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        OnSquadCreate?.Invoke(side);
    }

    void Spawn() {
        GameObject newMeeb = Instantiate(spawnObject, transform.position+transform.forward, Quaternion.identity) as GameObject;
        NPC_Behavior npc = newMeeb.GetComponent<NPC_Behavior>();
        npc.side=side;
    }

    void ResetSpawn() {
        isSpawning=true;
    }

}
