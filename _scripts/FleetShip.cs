using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetShip : Photon.PunBehaviour
{
    public int hp;
    public int popHeld;
    public int foodHeld;
    public int fuelHeld;
    public int targetJumpCords;


    public GameObject placeHolderInFleet;
    public float ftlTimer;
    public float ftlCost;
    public bool jumpOrdered;
    public GameObject fleetParent;
    public GameObject jumpTarget;
    public GameObject localSpaceObject;
    public Vector3 myPlaceInTheFleetFormation;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ftlTimer <= ftlCost) { ftlTimer += Time.deltaTime; }
        //if (Input.GetKeyDown(KeyCode.U))
            if (jumpOrdered == true && ftlTimer >= ftlCost)
          
            { ftlTimer = 0; GetComponent<PhotonView>().RPC("Jump", PhotonTargets.AllBufferedViaServer); }

    }
    public void OrderJump(int jumpCoords)
    {

        targetJumpCords = jumpCoords;
        ftlTimer = 0;
        jumpOrdered = true;
    }

    [PunRPC]
    public void Jump() {
        //Vector3 localObject = localSpaceObject.transform.localPosition;
        //localSpaceObject.transform.parent = fleetParent.GetComponent<Fleet>().jumpDestination.transform;
        //localSpaceObject.transform.localPosition = localObject;
        //transform.position = localSpaceObject.transform.position;
       
            fleetParent.GetComponent<Fleet>().shipsJumped++;
            GetComponent<FTLDrive>().currentCords = targetJumpCords;
            ftlTimer = 0;
            jumpOrdered = false;
        
        
        
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet" && hp > 0)
        {
            hp--;
            if (hp <= 0) { GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllBufferedViaServer); }
            //Destroy(this.gameObject);
            Debug.Log("hit");
        }
    }
    [PunRPC]
    public void Die()
    {
        fleetParent.GetComponent<Fleet>().UpdateResources(-fuelHeld,-foodHeld,-1,-popHeld,-1); ;

        Destroy(this.gameObject);
    }
}
