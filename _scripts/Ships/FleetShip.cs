using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public bool jumpReady;
    public bool jumpOrdered;
    public bool jumped;
    public GameObject fleetParent;
    public GameObject jumpTarget;
    public GameObject localSpaceObject;
    public Vector3 myPlaceInTheFleetFormation;
    public bool leftBehind;
    public GameObject dradisModel;
    public bool allowedInFleet; //toggle from admiral menu. To disallow a ship to jump with the fleet.

    public GameObject myMenuButton;
    public Text currentStatus;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ftlTimer <= ftlCost)
        {
            ftlTimer += Time.deltaTime;
        }
        if (jumpReady == false && leftBehind == false)
        {
            if (ftlTimer >= ftlCost)
            {
                jumpReady = true; fleetParent.GetComponent<Fleet>().ShipReportingFTLReady();
                currentStatus.text = "FTL Ready";
            }
        }
        //if (Input.GetKeyDown(KeyCode.U))
            if (jumpOrdered == true && ftlTimer >= ftlCost)
          
            {
            Jump();
           // ftlTimer = 0; GetComponent<PhotonView>().RPC("Jump", PhotonTargets.AllBufferedViaServer);
        }
       


    }
 

    public void OrderJump(int jumpCoords)
    {

        targetJumpCords = jumpCoords;
        //ftlTimer = 0;
       // jumped = false;
        jumpOrdered = true;
    }

    //[PunRPC]
    public void Jump() {
        currentStatus.text = "";
        jumpOrdered = false;
        fleetParent.GetComponent<Fleet>().shipsJumped++;
            GetComponent<FTLDrive>().currentCords = targetJumpCords;
            ftlTimer = 0;
        jumpReady = false;
     jumped = true;
        
        
        
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet" && hp > 0)
        {
        //    hp--;
           
        //    if (hp <= 0) {
        //        Die();
        //        //GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllBufferedViaServer);
        //    }
            //Destroy(this.gameObject);
            Debug.Log("hit");
        }
    }
    public void LeftBehind()
    {
        currentStatus.text = "Left Behind";

        myMenuButton.GetComponent<Button>().enabled = false;
        myMenuButton.GetComponent<Image>().color = Color.black;

    }
    public void AllowInFleetToggle()
    {
        //todo: just a toggle is too powerful for hidden cylons to abuse
        if (allowedInFleet == true)
        { allowedInFleet = false;
            myMenuButton.GetComponent<Image>().color = Color.red;
        }
        else { allowedInFleet = true;
            myMenuButton.GetComponent<Image>().color = Color.green;
        }
    }

    public void FleetShipDie()
    {
        fleetParent.GetComponent<Fleet>().UpdateResources(-fuelHeld,-foodHeld,-1,-popHeld,-1); ;
        myMenuButton.GetComponent<Image>().color = Color.black;
        myMenuButton.GetComponent<Button>().enabled = false;
        currentStatus.text = "Destroyed";
        //Destroy(this.gameObject);
    }

    public void RemoveFleetResources()
    {
        fleetParent.GetComponent<Fleet>().UpdateResources(-fuelHeld, -foodHeld, -1, -popHeld, -1);
    }

    public void AddFleetResources()
    {
        fleetParent.GetComponent<Fleet>().UpdateResources(+fuelHeld, +foodHeld, +1, +popHeld, +1);
    }
}
