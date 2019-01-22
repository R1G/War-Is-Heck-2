using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
    public Text endScreenText;
    public Text endScreenSubtitle;

    const string WinTitle = "You Win";
    const string LoseTitle = "You Lose";
    const string DeathMatchWin = "Enemy team eliminated";
    const string DeathLose ="You died";
    const string SiegeWin = "Enemy base destroyed";
    const string SiegeLose = "Your base was destroyed";

    public void SetEndText(GameManager.Clauses side, GameManager.Clauses reason) {
        switch((int)side*(int)reason) {
            //Red destroys blue base
            case ((int)GameManager.Clauses.Red*(int)GameManager.Clauses.Siege): { 
                endScreenText.text=LoseTitle;
                endScreenSubtitle.text=SiegeLose;
                break;
            }
            
            //Red kills player
            case ((int)GameManager.Clauses.Red*(int)GameManager.Clauses.Death): {
                endScreenText.text=LoseTitle;
                endScreenSubtitle.text=DeathLose;
                break;
            }
            
            //Blue destroys base
            case ((int)GameManager.Clauses.Blue*(int)GameManager.Clauses.Siege): {
                endScreenText.text=WinTitle;
                endScreenSubtitle.text=SiegeWin;
                break;
            }
           
            //Blue kills enemy team
            case((int)GameManager.Clauses.Blue*(int)GameManager.Clauses.Deathmatch): {
                endScreenText.text=WinTitle;
                endScreenSubtitle.text=DeathMatchWin;
                break;
            }
        }

    }
}
