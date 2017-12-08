using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfShip : MonoBehaviour {
    public GameObject myShip;
    public int shipSize;
    public GameObject lastCollision;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
           // myShip.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);
           myShip.SendMessage("TakeDamage",1);
            Debug.Log("hit");
        }
        if (col.gameObject.tag == "LargeShipCollider")
        {
          // if(col.gameObject.GetComponent<LargeShip>().size >= myShip.GetComponent<LargeShip>().size)
          //  myShip.SendMessage("Impact",col.gameObject.transform.position);
           
        }
    }
    public void OnTriggerEnter(Collider col2)
    {

       
            if (col2.gameObject.tag == "LargeShipCollider")
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
