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
    // Use this for initialization
    void Start () {
		
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
            { if (gunCooldown <= 0) { gunCooldown = 0.5f; Fire(); } }
       
    }
    public void Fire()
    {
        Instantiate(bullet,gun.transform.position,gun.transform.rotation);
    }
    public void OnTriggerStay(Collider col)
    {

        
        if (target == null)
        {
            if (col.gameObject.tag == "Raider") {
                target = col.transform.parent.gameObject;
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
