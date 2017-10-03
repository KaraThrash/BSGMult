using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raider : Photon.PunBehaviour
{
    public int speed;
    public GameObject patrolPointsParent;
    public GameObject[] points;
    public GameObject patrolTarget;
    public GameObject shipTarget;
    public int currentPoint = 0;
    private Quaternion targetRotation;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float gunCooldown;
    private Rigidbody rb;
    private int strafeDirection;
    public string patrolPointType;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        if (Random.Range(0, 2) == 1) { strafeDirection = 1; } else { strafeDirection = -1; }
        patrolPointsParent = GameObject.Find(patrolPointType);
        foreach (Transform child in patrolPointsParent.transform)
        {
            if (currentPoint < points.Length)
            {
                points[currentPoint] = child.gameObject;
                currentPoint++;
            }
           
        }
        patrolTarget = points[0];
        currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (shipTarget == null  ) { Patrol(); }
        if (shipTarget != null) {  Attack(); if (Vector3.Distance(transform.position, shipTarget.transform.position) > 3000) { shipTarget = null; } }
	}
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Destroy(this.gameObject);
            Debug.Log("hit");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
       // Debug.Log(other.transform.name);
       //TODO: fix border to just be an actual border instead of full sphere
        if (other.tag == "Viper") { if (shipTarget == null && other.transform.parent.gameObject.GetComponent<ViperControls>().flying == true) {shipTarget = other.transform.parent.gameObject; } }
        if (other.tag == "Fleetship") { if (shipTarget == null) { shipTarget = other.transform.parent.gameObject; } }
    }
    public void FireGuns()
    {
        Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
        Instantiate(bullet, gun1.transform.position, gun1.transform.rotation);
    }

    public void Attack()
    {
        gunCooldown -= Time.deltaTime;
        targetRotation = Quaternion.LookRotation(shipTarget.transform.position - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 6 * Time.deltaTime);

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
    public void Patrol() {
        targetRotation = Quaternion.LookRotation(patrolTarget.transform.position - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 6 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget.transform.position, speed * Time.deltaTime);
        if (Vector3.Distance(patrolTarget.transform.position, transform.position) < 5) { GotoNextPoint(); }
    }
    void GotoNextPoint()
    {
        if (points.Length == 0)
        { return; }
        currentPoint = (currentPoint + 1) % points.Length;
        patrolTarget = points[currentPoint];
        
    }
    [PunRPC]
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
