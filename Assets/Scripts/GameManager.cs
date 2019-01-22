using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> blueTeam = new List<GameObject>();
    public List<GameObject> redTeam = new List<GameObject>();
    public GameObject player;
    bool gameOver;

    public int redCount=0;
    public int blueCount=0;
    public enum Clauses {Deathmatch=1, Siege=2, Death=3, Red=10, Blue=100};
    void Start()
    {
        Instantiate(Resources.Load("StartgameUI"));
        player = GameObject.FindGameObjectWithTag("Player");
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

    public void KillPlayer() {
        GameObject blueBase = GameObject.Find("BlueBase");
        player = Instantiate(player, blueBase.transform.position+Vector3.forward, Quaternion.identity);
    }

    void EndMatch(Clauses side, Clauses reason) {
        if(gameOver) {
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
