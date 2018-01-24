using System.Collections;
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
    public bool destroyed;
    public int hp;
    public int accuracy; //difference of angle that the raider can start firing 0 being perfect
    public float gunCost; //what to set the cooldown to after firing
    // Use this for initialization
    void Start () {
        hp = 1;
        rb = GetComponent<Rigidbody>();
        myWing = transform.parent.gameObject;
        if (Random.Range(0, 3) == 1) { strafeDirection = 1; } else { if (Random.Range(0, 3) == 2) { strafeDirection = -1; } else { strafeDirection = 0; } }
        
    }

    // Update is called once per frame
    void Update() {
        // CheckForward();
        if (myWing.GetComponent<FighterWing>().shipTarget != null)
        { shipTarget = myWing.GetComponent<FighterWing>().shipTarget; }
        else { shipTarget = null; }

        if (shipTarget != null)
        {
            Attack();
        }
        else
        {
            if (avoidCollisionClock <= 0f)
            {

                

                Patrol(); CheckForward(); 

            }
            else { AvoidCollision(); }
        }
            
    
	}
    public void CheckForward()
    {
       
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50.0f) )
        {
          
                if (avoidCollisionClock < 0) { avoidCollisionClock = 0.4f; }
                else { if (avoidCollisionClock < 3) { avoidCollisionClock += Time.deltaTime; } }
            

        }
        else { avoidCollisionClock -= 0.1f;  }
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
        float angle = Vector3.Angle(shipTarget.transform.position - transform.position, transform.forward);
        if (angle <= accuracy) { canShoot = true; } else { canShoot = false; }


        
        if (Vector3.Distance( transform.position, shipTarget.transform.position) > 100 || Vector3.Distance(transform.position, shipTarget.transform.position) < 30)
        { transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, speed * Time.deltaTime); }
        
        // transform.position += (transform.forward * Time.deltaTime * speed);
        transform.position += (transform.right * speed * strafeDirection * Time.deltaTime);

            if ( gunCooldown <= 0 && canShoot == true)
            {
                FireGuns();
            gunCooldown = gunCost + Random.Range(0,3.0f);

            }
        
    }
    public void Patrol()
    {


        if (Vector3.Distance(transform.position, myPlaceInWing.transform.position) > 55)
        {
            transform.position = Vector3.MoveTowards(transform.position, myPlaceInWing.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, myPlaceInWing.transform.rotation, speed * Time.deltaTime);
        }
  
        
    }

  

 
    public void TakeDamage(int dmg, int fromWho)
    {
      

        Instantiate(explosion, transform.position, transform.rotation);
        if (destroyed == false)
        {
            hp -= dmg;
            if (hp <= 0)
            {
               
                destroyed = true;

                myWing.GetComponent<PhotonView>().RPC("ShipDestroyed", PhotonTargets.AllViaServer, myNumber,fromWho);
                



            }
           
        }
    
    }

}
