using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject blueBase;
    public GameObject redBase;


    public List<GameObject> blueTeam = new List<GameObject>();
    public List<GameObject> redTeam = new List<GameObject>();


    //TODO: Add randomized squad names and memoirs when they die :')
    IDictionary<int, List<GameObject>> blueSquads = new Dictionary<int, List<GameObject>>();
    public List<GameObject> currentBlueSquad = new List<GameObject>();
    int currentBlueIndex=0;

    IDictionary<int, List<GameObject>> redSquads = new Dictionary<int, List<GameObject>>();
    public List<GameObject> currentRedSquad = new List<GameObject>();
    int currentRedIndex=0;


    public GameObject player;


    bool gameOver;
    public bool matchMode;


    public int redCount=0;
    public int blueCount=0;

    public enum Clauses {Deathmatch=1, Siege=2, Death=3, Red=10, Blue=100};
    public enum Side {Red, Blue};

    void Start()
    {
        Instantiate(Resources.Load("StartgameUI"));
        player = GameObject.FindGameObjectWithTag("Player");
        blueTeam.Add(player);
        gameOver=false;
    }

    public void KillRed(GameObject red) {
        redTeam.Remove(red);
        redCount--;
        if(redTeam.Count<=0) {
            EndMatch(Clauses.Blue, Clauses.Deathmatch);
        }
    }

    public void KillBlue(GameObject blue) {
        blueCount--;
        redTeam.Remove(blue);
    }

    public void DestroyBlueBase() {
        EndMatch(Clauses.Red, Clauses.Siege);
    }

    public void DestroyRedBase() {
        EndMatch(Clauses.Blue, Clauses.Siege);
    }

    public void UpdateSquads(GameManager.Side side) {
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

    public void AddToSquad(GameManager.Side side, GameObject newMeeb) {
        if(side==GameManager.Side.Blue) {
            currentBlueSquad.Add(newMeeb);
        } else if(side==GameManager.Side.Red) {
            currentRedSquad.Add(newMeeb);
        }
    }

    public int GetSquadIndex(GameManager.Side side) {
        return (side==GameManager.Side.Blue) ? currentBlueIndex : currentRedIndex;
    }

    public List<GameObject> GetSquad(GameManager.Side side, int index) {
        Debug.Log(index);
        return (side==GameManager.Side.Blue) ? blueSquads[index] : redSquads[index];
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
