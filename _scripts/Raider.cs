﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : MonoBehaviour
{
    public int myNumber; //in wing for destroying
    public GameObject fwdObject;
    public int speed;
    public int rotForce = 6;
    public GameObject myPlaceInWing;

    public GameObject shipTarget;
    private Quaternion targetRotation;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float gunCooldown;
    private Rigidbody rb;
    private int strafeDirection;
    public string patrolPointType;
    public bool canPatrol;
    public GameObject explosion;
    public GameObject dradisModel;
    public float avoidCollisionClock;
    public bool canShoot;
    public GameObject myWing;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        myWing = transform.parent.gameObject;
        if (Random.Range(0, 2) == 1) { strafeDirection = 1; } else { strafeDirection = -1; }
        
    }
	
	// Update is called once per frame
	void Update () {
        CheckForward();



        if (avoidCollisionClock <= 0f)
        {



            shipTarget = myWing.GetComponent<FighterWing>().shipTarget;
            if (Vector3.Distance(myPlaceInWing.transform.position, transform.position) > 500 )
            {
                //deactivate
                Patrol();
                
            }
            else
            {
                if (shipTarget == null) { Patrol(); }
                if (shipTarget != null) { Attack(); }
            }

        }
        else { AvoidCollision(); }
        
	}
    public void CheckForward()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 110.0f) )
        {
            if (hit.transform.gameObject == shipTarget)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
                if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
                else { if (avoidCollisionClock < 3) { avoidCollisionClock += Time.deltaTime; } }
            }

        }
        else { avoidCollisionClock -= 0.1f; canShoot = true; }
    }
    public void AvoidCollision()
    {
        transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, speed * Time.deltaTime);
       // transform.position += (transform.forward * Time.deltaTime * speed);
        
        transform.Rotate(Vector3.right * rotForce * avoidCollisionClock * strafeDirection * Time.deltaTime);
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            GameObject.Find("RoundManager").GetComponent<RoundManager>().CylonKilled(1, col.gameObject.GetComponent<Bullet>().owner);
            //Destroy(this.gameObject);
            Die();
            Debug.Log("hit");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
       // Debug.Log(other.transform.name);
       //TODO: fix border to just be an actual border instead of full sphere
       // if (other.tag == "Viper") { if (shipTarget == null && other.transform.parent.gameObject.GetComponent<ViperControls>().flying == true) {shipTarget = other.transform.parent.gameObject; } }
       // if (other.tag == "Fleetship") { if (shipTarget == null) { shipTarget = other.transform.parent.gameObject; } }
    }
    public void FireGuns()
    {

        GameObject clone = Instantiate(bullet, gun2.transform.position, gun2.transform.rotation) as GameObject;
        clone.GetComponent<Rigidbody>().velocity += rb.velocity;
        GameObject clone1 = Instantiate(bullet, gun1.transform.position, gun2.transform.rotation) as GameObject;
        clone1.GetComponent<Rigidbody>().velocity += rb.velocity;
    }

    public void Attack()
    {
        transform.parent = null;
        gunCooldown -= Time.deltaTime;
        targetRotation = Quaternion.LookRotation(shipTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);

        Debug.Log("In the if " + Vector3.Distance(transform.position, shipTarget.transform.position));
        if (Vector3.Distance( transform.position, shipTarget.transform.position) > 100 || Vector3.Distance(transform.position, shipTarget.transform.position) < 20)
        { transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, speed * Time.deltaTime); }
        Debug.Log("Not In the if " + Vector3.Distance(transform.position, shipTarget.transform.position));
        // transform.position += (transform.forward * Time.deltaTime * speed);
        transform.position += (transform.right * speed * strafeDirection * Time.deltaTime);

            if ( gunCooldown <= 0 && canShoot == true)
            {
                FireGuns(); gunCooldown = 0.5f;

            }
        
    }
    public void Patrol()
    {
        
       // if (Vector3.Distance(myPlaceInWing.transform.position, transform.position) > 10)
        //{ transform.position = Vector3.MoveTowards(transform.position, myPlaceInWing.transform.position, speed * Time.deltaTime); }
        transform.position = Vector3.MoveTowards(transform.position, myPlaceInWing.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, myPlaceInWing.transform.rotation, speed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, myPlaceInWing.transform.position, speed * Time.deltaTime);
        
    }

    // [PunRPC]
    public void Die()
    {
        //GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);
        myWing.GetComponent<PhotonView>().RPC("ShipDestroyed", PhotonTargets.AllViaServer, myNumber);
        //Destroy(this.gameObject);
    }



}
