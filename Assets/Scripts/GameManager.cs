using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject blueBase, redBase;

    private List<GameObject> blueTeam = new List<GameObject>();
    private List<GameObject> redTeam = new List<GameObject>();


    //TODO: Add randomized squad names and memoirs when they die :')
    private IDictionary<int, List<GameObject>> blueSquads = new Dictionary<int, List<GameObject>>();
    public List<GameObject> currentBlueSquad = new List<GameObject>();
    int currentBlueIndex=0;

    IDictionary<int, List<GameObject>> redSquads = new Dictionary<int, List<GameObject>>();
    public List<GameObject> currentRedSquad = new List<GameObject>();
    int currentRedIndex=0;


    private GameObject player;
    private bool gameOver;

    public bool matchMode;
    public int redCount, blueCount=0;

    public enum Clauses {Deathmatch=1, Siege=2, Death=3, Red=10, Blue=100};
    public enum Side {Red, Blue};
    public enum Weapon {Machete, PillGun};

    private void OnEnable() {
        Spawner.OnSpawnerCreate += AddSpawner;
        NPC_Behavior.OnCreateNPC += AddNPC;
        Player.OnCreatePlayer += AddPlayer;
        Spawner.OnSquadCreate += UpdateSquads;
    }

    private void OnDisable() {
        Spawner.OnSpawnerCreate -= AddSpawner;
        NPC_Behavior.OnCreateNPC -= AddNPC;
        Player.OnCreatePlayer -= AddPlayer;
        Spawner.OnSquadCreate -= UpdateSquads;
    }

    void Start() {
        Instantiate(Resources.Load("StartgameUI"));
        player = GameObject.FindGameObjectWithTag("Player");
        blueTeam.Add(player);
        gameOver=false;
    }

    private void AddNPC(NPC_Behavior npc) {
        if(npc.side==Side.Red) {
            redTeam.Add(npc.gameObject);
        } else if(npc.side==Side.Blue) {
            blueTeam.Add(npc.gameObject);
        }
        AddToSquad(npc.side, npc.gameObject);
    }

    private void AddPlayer(GameObject _player) {
        player = _player;
    }

    private void KillRed(GameObject red) {
        redTeam.Remove(red);
        redCount--;
    }

    private void KillBlue(GameObject blue) {
        blueCount--;
        redTeam.Remove(blue);
    }

    private void AddSpawner(Spawner s) {
        if(s.side == Side.Blue) 
            blueBase = s.gameObject;
        if(s.side == Side.Red) 
            redBase = s.gameObject;
    }
    
    public GameObject GetEnemySpawner(Side s) {
        return (s==Side.Red)? blueBase : redBase;
    }

    public void DestroyBlueBase() {
        EndMatch(Clauses.Red, Clauses.Siege);
    }

    public void DestroyRedBase() {
        EndMatch(Clauses.Blue, Clauses.Siege);
    }

    public List<GameObject> GetTeam(Side s) {
        return (s==Side.Red)? redTeam : blueTeam;
    }

    private void UpdateSquads(GameManager.Side side) {
        if(side==GameManager.Side.Blue) {
            blueSquads.Add(currentBlueIndex, currentBlueSquad);
            currentBlueSquad = new List<GameObject>();
            currentBlueIndex+=1;
        } else if(side==GameManager.Side.Red) {
            redSquads.Add(currentRedIndex, currentRedSquad);
            currentRedSquad = new List<GameObject>();
            currentRedIndex+=1;
        }
    }

    private void AddToSquad(GameManager.Side side, GameObject newMeeb) {
        if(side==GameManager.Side.Blue) {
            currentBlueSquad.Add(newMeeb);
        } else if(side==GameManager.Side.Red) {
            currentRedSquad.Add(newMeeb);
        }
    }

    public List<GameObject> GetSquad(GameManager.Side side) {
        return (side==GameManager.Side.Blue) ? currentBlueSquad : currentRedSquad;
    }

    public void KillPlayer() {
        GameObject blueBase = GameObject.Find("BlueBase");
        player = Instantiate(player, blueBase.transform.position+Vector3.forward, Quaternion.identity);
        blueTeam.Add(player);
    }

    void EndMatch(Clauses side, Clauses reason) {
        if(gameOver || !matchMode) {
            return;
        }
        gameOver=true;
        GameObject endgameUI = Instantiate(Resources.Load("EndgameUI")) as GameObject;
        endgameUI.GetComponent<Endgame>().SetEndText(side, reason);
        Invoke("GoBackToStartMenu", 5f);
        //DisableAllScripts();
    }

    void GoBackToStartMenu() {
        SceneManager.LoadScene(0);
    }

}
