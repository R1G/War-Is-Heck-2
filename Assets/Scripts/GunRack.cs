using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRack : MonoBehaviour
{
    GameObject weapon;

    private void OnMouseOver() {
        if(Input.GetKey(KeyCode.F)) {
            Debug.Log("Clicked");
        }
    }
}
