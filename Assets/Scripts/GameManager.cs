using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameObject[] redTeam;

    static int redCount=0;
    public enum Clauses {Deathmatch=1, Siege=2, Death=3, Red=10, Blue=100};
    void Start()
    {
        redTeam = GameObject.FindGameObjectsWithTag("RED");
        redCount = redTeam.Length;
    }

    public static void KillRed() {
        redCount--;
        if(redCount<=0) {
            EndMatch(Clauses.Blue, Clauses.Deathmatch);
        }
    }

    public static void DestroyBlueBase() {
        EndMatch(Clauses.Red, Clauses.Siege);
    }

    public static void DestroyRedBase() {
        EndMatch(Clauses.Blue, Clauses.Siege);
    }

    public static void KillPlayer() {
        EndMatch(Clauses.Red, Clauses.Death);
    }

    static void EndMatch(Clauses side, Clauses reason) {
        GameObject endgameUI = Instantiate(Resources.Load("EndgameUI")) as GameObject;
        endgameUI.GetComponent<Endgame>().SetEndText(side, reason);
        DisableAllScripts();
    }

    static void DisableAllScripts() {
        GameObject[] reds = GameObject.FindGameObjectsWithTag("RED"); 
        GameObject[] blues = GameObject.FindGameObjectsWithTag("BLUE");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        foreach(GameObject red in reds) {
            Behaviour[] scripts = red.GetComponents<Behaviour>();
            foreach(Behaviour script in scripts){
                script.enabled = false;
            }
        }

        foreach(GameObject blue in blues) {
            Behaviour[] scripts = blue.GetComponents<Behaviour>();
            foreach(Behaviour script in scripts){
                script.enabled = false;
            }
        }

        if(player!=null) {
            Behaviour[] scripts = player.GetComponents<Behaviour>();
            foreach(Behaviour script in scripts){
                script.enabled = false;
            }
        }
    }

}
