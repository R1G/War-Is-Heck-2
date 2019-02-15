using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float radius;
    public float damage;
    public float force;
    GameObject target;
    bool hasExploded;
    // Start is called before the first frame update
    void Start()
    {
        hasExploded=false;
    }

    // Update is called once per frame
    void OnCollisionEnter(Collision coll)
    {
        if(!hasExploded) {
            target=coll.gameObject;
            Explode();
            hasExploded=true;
        }
    }

    void Explode() {
        Debug.Log(target);
        target.SendMessage("TakeDamage", 1000, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}
