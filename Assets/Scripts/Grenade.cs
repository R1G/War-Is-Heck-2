using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float fuseTime;
    public float radius;
    public float damage;
    public float force;
    public GameObject explodeFX;
    float countdown;
    bool hasExploded;
    // Start is called before the first frame update
    void Start()
    {
        countdown = fuseTime;
        hasExploded=false;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown<=0 && !hasExploded) {
            Explode();
            hasExploded=true;
        }
    }

    void Explode() {
        Instantiate(explodeFX, transform.position, Quaternion.identity);
        Collider[] colls = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider coll in colls)
        {
            Rigidbody rb = coll.gameObject.GetComponent<Rigidbody>();
            if(rb!=null) {
                rb.AddExplosionForce(force, transform.position, radius);
                coll.gameObject.SendMessage("TakeDamage", 1000, SendMessageOptions.DontRequireReceiver);
            }

            //Damage
        }
        Destroy(gameObject);
    }
}
