using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : MonoBehaviour
{
    public int myNumber; //in wing for destroying
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
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        if (Random.Range(0, 2) == 1) { strafeDirection = 1; } else { strafeDirection = -1; }
        
    }
	
	// Update is called once per frame
	void Update () {
        CheckForward();
        shipTarget = transform.parent.GetComponent<FighterWing>().shipTarget;
        if (avoidCollisionClock <= 0f)
        {
            if (Vector3.Distance(myPlaceInWing.transform.position, transform.position) > 200) {
                //deactivate
                transform.position = Vector3.MoveTowards(transform.position, myPlaceInWing.transform.position, speed * Time.deltaTime);
            }
            else
            {
                if (shipTarget == null) { Patrol(); }
                if (shipTarget != null) { Attack(); if (Vector3.Distance(transform.position, shipTarget.transform.position) > 3000) { shipTarget = null; } }
            }

        }
        else { AvoidCollision(); }

	}
    public void CheckForward()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f) && hit.transform.gameObject != shipTarget)
        { if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
            else { if (avoidCollisionClock < 3) { avoidCollisionClock += Time.deltaTime; }  }
            

        }
        else { avoidCollisionClock -= 0.1f; }
    }
    public void AvoidCollision()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime,ForceMode.Impulse);
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
        gunCooldown -= Time.deltaTime;
        targetRotation = Quaternion.LookRotation(shipTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotForce * Time.deltaTime);
        //float newangle = Quaternion.Angle(transform.rotation, shipTarget.transform.rotation);
        //Debug.Log(newangle);
        //if (newangle < 240 && newangle > 80)
        
        //{ rb.AddTorque((transform.position - shipTarget.transform.position) * rotForce * Time.deltaTime); }
     

        if (Vector3.Distance(shipTarget.transform.position, transform.position) > 70)
        {
            rb.AddForce(transform.forward * speed * 250 * Time.deltaTime);
            // transform.position = Vector3.MoveTowards(transform.position, shipTarget.transform.position, speed * Time.deltaTime);

        }
        else
        {
            //TODO: make AI vipers do what the pegasus vipers do in the fight vs galactica. the rigid hard diretion forward than turn

            rb.AddForce(transform.forward * speed  * Time.deltaTime);
            rb.AddForce(transform.right * speed * 150 * strafeDirection * Time.deltaTime);

            if ( gunCooldown <= 0)
            {
                FireGuns(); gunCooldown = 0.5f;

            }
        }
    }
    public void Patrol()
    {
        
        

        transform.rotation = Quaternion.Lerp(transform.rotation, myPlaceInWing.transform.rotation, 1 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, myPlaceInWing.transform.position, 1 * Time.deltaTime);
        
    }

    // [PunRPC]
    public void Die()
    {
        //GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);
        transform.parent.GetComponent<PhotonView>().RPC("ShipDestroyed", PhotonTargets.AllBufferedViaServer, myNumber);
        //Destroy(this.gameObject);
    }



}
