using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetShip : Photon.PunBehaviour
{
    public int hp;
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
        jumpTarget = GameObject.Find(jumpCoords.ToString()); 
       // transform.position = jumpTarget.transform.position;

        Vector3 localObject = localSpaceObject.transform.localPosition;
        localSpaceObject.transform.parent = jumpTarget.transform;
        localSpaceObject.transform.localPosition = localObject;
        transform.position = localSpaceObject.transform.position;
        fleetParent.GetComponent<Fleet>().shipsJumped++;
        ftlTimer = 0;
        jumpOrdered = false;
    }

    [PunRPC]
    public void Jump() {
        Vector3 localObject = localSpaceObject.transform.localPosition;
        localSpaceObject.transform.parent = fleetParent.GetComponent<Fleet>().jumpDestination.transform;
        localSpaceObject.transform.localPosition = localObject;
        transform.position = localSpaceObject.transform.position;
        fleetParent.GetComponent<Fleet>().shipsJumped++;
        ftlTimer = 0;
        jumpOrdered = false;
    }
}
