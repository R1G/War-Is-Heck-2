using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startgame : MonoBehaviour
{
    public RawImage blackScreen;
    public Text objectiveText;
    private int blackScreenAlpha=50; //percentage
    private int transitionSpeed=1;

    void Start() {
        InvokeRepeating("FadeUI", 0f, .05f);
    }
    void FadeUI()
    {
        blackScreen.color = new Color(0, 0, 0, (blackScreenAlpha/100f));
        objectiveText.color = new Color(255, 255, 255, (blackScreenAlpha/100f));
        if(blackScreenAlpha>0) {
            blackScreenAlpha-=transitionSpeed;
        } else {
            Destroy(gameObject);
        }
    }
}
