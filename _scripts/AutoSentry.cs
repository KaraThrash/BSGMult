using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSentry : MonoBehaviour {
    public int myFaction;
    public GameObject turret;
    public GameObject gun;
    public GameObject bullet;
    public GameObject target;
    public float gunCooldown;
    public string targetTag;
    public float fireRate;
    public float moveForwardSpeed;
    public bool spawnedByHeavyRaider; //TODO: find better solution for them stacking on top of each other
    // Use this for initialization
    void Start () {
        if (spawnedByHeavyRaider == true)
        { GetComponent<Rigidbody>().velocity = transform.forward * moveForwardSpeed * Time.deltaTime; }
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            TrackTarget();
            gunCooldown -= Time.deltaTime;
        }
	}
    public void TrackTarget()
    {
        Vector3 targetDir = target.transform.position - turret.transform.position;

        Vector3 newDir = Vector3.RotateTowards(turret.transform.forward, targetDir, 2 * Time.deltaTime, 0.0F);

        turret.transform.rotation = Quaternion.LookRotation(newDir);


        float angle = Vector3.Angle(targetDir, turret.transform.forward);
        Debug.Log(angle + "angle");
        if (angle < 5.0f )
            { if (gunCooldown <= 0) { gunCooldown = fireRate; Fire(); } }
       
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
    }

    public void Fire()
    {
        target = null;
        Instantiate(bullet,gun.transform.position,gun.transform.rotation);
    }
    public void OnTriggerStay(Collider col)
    {

        
        if (target == null)
        {
            if (col.gameObject.tag == targetTag) {
                target = col.transform.gameObject;
               // if (col.transform.parent.GetComponent<Fighter>().faction != myFaction)
               // { }
               
            }
        }

    }
    public void OnTriggerExit(Collider col)
    {


        if (col == target)
        {
            target = null;
        }

    }
}
