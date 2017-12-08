using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRaider : MonoBehaviour {
   
    public GameObject cargo; //centurions, bombs?
    public GameObject spawnLocation;
    public bool hasCargo;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "LargeShipCollider")
           
        {
            if (hasCargo == true && col.gameObject.GetComponent<PartOfShip>() != null)
            {
                GetComponent<Missile>().enabled = false;
                   GameObject breachObject =  col.gameObject.GetComponent<PartOfShip>().myShip.GetComponent<LargeShip>().breachLocation;
                hasCargo = false;
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = breachObject.transform;
                transform.position = breachObject.transform.position;
                transform.rotation = breachObject.transform.rotation;
                GameObject clone = Instantiate(cargo, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;
                clone.transform.parent = breachObject.transform;
            }
        }

        if (col.gameObject.tag == "Player" && GetComponent<Rigidbody>().isKinematic == false) { col.gameObject.SendMessage("TakeDamage", 10); }
    }
 
}
