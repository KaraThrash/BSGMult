using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fleet : Photon.PunBehaviour
{
    public int fuel;
    public int pop;
    public int morale;
    public int food;
    public int shipsInFleetCount;
    public int shipsReadyToJump;
    public int shipsJumped;
    public int jumpCoordinates;
    public GameObject jumpManager;
    public GameObject jumpDestination;
    public Vector3 jumpLocation;
    public int fuelCost;
    public bool jumping;
   
    public Text shipsJumpedText;
    public GameObject resourceTextObj;
    public Text foodText;
    public Text fuelText;
    public Text moraleText;
    public Text popText;
    public List<GameObject> shipsInFleet = new List<GameObject>();
    public GameObject activeFleet;
    public GameObject shipsLeftBehind;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (jumping == true)
        {
           // shipsJumpedText.text = shipsJumped.ToString() + "/" + shipsInFleetCount.ToString() + " Currently jumped away.";
            if (shipsJumped >= shipsInFleet.Count)
            {
                //jumping = false;
                //shipsJumped = 0;
                //shipsJumpedText.text = "Fleet Away";

                
            }
        }

        if (Input.GetKey(KeyCode.LeftShift)) { foodText.text = food.ToString(); fuelText.text = fuel.ToString();
            moraleText.text = morale.ToString(); popText.text = pop.ToString(); resourceTextObj.active = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        { resourceTextObj.active = false; }
    }
    public void ShipReportingFTLReady()
    {
        shipsReadyToJump++;
        shipsJumpedText.text = shipsReadyToJump.ToString() + " of "+ shipsInFleetCount.ToString() + " Ships Ready To Jump";
    }

    [PunRPC]
    public void Jumping(int jumpCoords) {
        GetComponent<FTLDrive>().currentCords = jumpCoords;
        jumpManager.GetComponent<PhotonView>().RPC("UpdateLocationFleet", PhotonTargets.AllBufferedViaServer, jumpCoords);
        jumpCoordinates = jumpCoords;
       // jumpDestination = GameObject.Find(jumpCoordinates.ToString());
        shipsJumped = 0;
        shipsReadyToJump = 0;
        jumping = true;
        for (var i = shipsInFleet.Count - 1; i >= 0; i--)
        {

           
                if (shipsInFleet[i] != null)
                {

                    shipsInFleet[i].GetComponent<FleetShip>().OrderJump(jumpCoordinates);
                    
                    //shipsInFleet[i].GetComponent<PhotonView>().RPC("StartJump", PhotonTargets.AllViaServer, jumpCoordinates);
                }
                else { shipsInFleet.RemoveAt(i); }
            
        }
    }
    public void CheckAllShipsLocation( )
    {
        for (var i = shipsInFleet.Count - 1; i >= 0; i--)
        {

           
                if (shipsInFleet[i] != null)
                {
                if (shipsInFleet[i].GetComponent<FleetShip>().jumped == true)
                {
                    shipsInFleet[i].active = true;
                    shipsInFleet[i].GetComponent<FleetShip>().jumped = false;
                }
                else {
                    shipsInFleet[i].transform.parent = shipsLeftBehind.transform;
                    if (shipsInFleet[i].GetComponent<FleetShip>().leftBehind == false)
                    { shipsInFleet[i].GetComponent<FleetShip>().LeftBehind(); }
                    else {
                        shipsInFleet[i].GetComponent<FleetShip>().FleetShipDie();

                        //GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllBufferedViaServer);
                        shipsInFleet.RemoveAt(i);
                    }
                        //.RemoveFleetResources();
                    //shipsInFleet[i].active = false;
                }
                    
                }
                else { shipsInFleet.RemoveAt(i); }
        }
        

    }




    public void UpdateResources(int foodChange, int fuelChange, int moraleChange, int popChange, int shipsChange )
    { food += foodChange; fuel += fuelChange; pop += popChange; morale += moraleChange;shipsInFleetCount += shipsChange;
        foodText.text = food.ToString(); fuelText.text = fuel.ToString();
        moraleText.text = morale.ToString(); popText.text = pop.ToString();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

          //  stream.SendNext(jumpDestination);
            //  stream.SendNext(rotationObject.transform.rotation);
        }
        else
        {
            // fwdObject.transform.position = (Vector3)stream.ReceiveNext();
           // jumpDestination = (GameObject)stream.ReceiveNext();
        }
    }
}
