using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRaider : Photon.PunBehaviour
{
   
    public GameObject cargo; //centurions, bombs, sentry guns?
    public GameObject spawnLocation;
    public string objectToSpawn;
    public bool hasCargo;
    public bool startClock;
    public float dieTime;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (startClock == true)
        { dieTime -= Time.deltaTime;if (dieTime <= 0) { Destroy(this.gameObject); } }
	}


    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "LargeShipCollider" && col.gameObject.GetComponent<PartOfShip>() != null)
           
        {
            if (hasCargo == true && col.gameObject.GetComponent<PartOfShip>().breachLocation != null)
            {
                GetComponent<Missile>().enabled = false;
                   GameObject breachObject =  col.gameObject.GetComponent<PartOfShip>().breachLocation;
               
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = breachObject.transform;
                transform.position = breachObject.transform.position;
                transform.rotation = breachObject.transform.rotation;
                if (photonView.isMine == true)
                { GameObject clone = PhotonNetwork.InstantiateSceneObject(objectToSpawn, spawnLocation.transform.position, spawnLocation.transform.rotation, 0, null); }
               // GameObject clone = Instantiate(cargo, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;

               // clone.transform.parent = breachObject.transform;
             
                startClock = true;
                
            }
        }

        //if (col.gameObject.tag == "Player" && GetComponent<Rigidbody>().isKinematic == false) { col.gameObject.SendMessage("TakeDamage", 10); }
    }
 
}
