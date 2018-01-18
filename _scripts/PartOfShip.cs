using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfShip : Photon.PunBehaviour
{
    public GameObject myShip;
    public int shipSize;
    public GameObject lastCollision;
    public bool cantCollide;
    public GameObject secondarySystem; //deal damage to specific system in that part of the ship // hit in the engines take damage to engine systems 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [PunRPC]
    public void TakeDamage(int dmg, int byWho)
    {
        if (dmg >= shipSize)
        {
                if (secondarySystem != null)
                {
                    //secondarySystem.SendMessage("TakeDamage", 1);
                    secondarySystem.GetComponent<PhotonView>().RPC("Damaged", PhotonTargets.AllViaServer);
                }
                myShip.SendMessage("TakeDamage", dmg);
        }
        
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            // myShip.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);
            if (col.gameObject.GetComponent<Bullet>().damage >= shipSize)
            {
                if (secondarySystem != null)
                {
                    //secondarySystem.SendMessage("TakeDamage", 1);
                    secondarySystem.GetComponent<PhotonView>().RPC("Damaged", PhotonTargets.AllViaServer);
                }
                myShip.SendMessage("TakeDamage", col.gameObject.GetComponent<Bullet>().damage);
                Debug.Log("hit");
            }
        }
        if (col.gameObject.tag == "LargeShipCollider")
        {
          // if(col.gameObject.GetComponent<LargeShip>().size >= myShip.GetComponent<LargeShip>().size)
          //  myShip.SendMessage("Impact",col.gameObject.transform.position);
           
        }
    }
    public void OnTriggerEnter(Collider col2)
    {

       
            if (col2.gameObject.tag == "LargeShipCollider" && cantCollide == false)
            {
            if (col2.gameObject.GetComponent<PartOfShip>().myShip == lastCollision) { lastCollision = null; }
            else
            {
                if (col2.gameObject.GetComponent<PartOfShip>().myShip != myShip)
                {
                    lastCollision = col2.gameObject.GetComponent<PartOfShip>().myShip;
                    if (col2.gameObject.GetComponent<PartOfShip>().shipSize >= shipSize)
                    { myShip.SendMessage("Impact", col2.gameObject.transform.position); }
                    myShip.SendMessage("TakeDamage", col2.gameObject.GetComponent<PartOfShip>().shipSize);
                }
            }
        }
    }
    public void OnTriggerExit(Collider col3)
    {

        if (col3.gameObject.tag == "LargeShipCollider")
        {
           
                myShip.SendMessage("ExitImpact");

        }
    }

    public void Interact(GameObject whoUsedMe)
    {
        


    }
    public void Repair(GameObject whoUsedMe)
    {
        myShip.SendMessage("Repair", whoUsedMe);

    }

    public void Sabotage(GameObject whoUsedMe)
    {
        myShip.SendMessage("Sabotage", whoUsedMe);

    }
    public void DealDamage(int dmg)
    {
        // for bombs to use
        myShip.SendMessage("TakeDamage", dmg);

    }
}
