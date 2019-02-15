using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spawnObject;
    GameManager gm;
    public float spawnRate;
    public int spawnSize;
    bool isSpawning = true;

    void Start() {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if(isSpawning && (gm.blueCount+gm.redCount)<=30) {
            SpawnSquad();
            Invoke("ResetSpawn", spawnRate);
        } 
    }

    void SpawnSquad() {
        for(int i=0; i<spawnSize; i++) {
            Invoke("Spawn", 0.2f*i);
        }
        isSpawning=false;
    }

    void Spawn() {
        Instantiate(spawnObject, transform.position+transform.forward, Quaternion.identity);
    }

    void ResetSpawn() {
        isSpawning=true;
    }
}
