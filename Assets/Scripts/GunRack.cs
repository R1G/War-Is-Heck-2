using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRack : MonoBehaviour
{
    public GameManager.Weapon weapon;
    GameObject player;

    void Update() {
        if(Input.GetKeyDown(KeyCode.F)) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(Vector3.Distance(player.transform.position, transform.position) <= 2f) {
                player.GetComponent<Combat>().weapon = weapon;
                player.GetComponent<Combat>().SetWeapon();
            }
        }
    }
}
