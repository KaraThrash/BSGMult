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
    public int shipsJumped;
    public int jumpCoordinates;
    public GameObject jumpDestination;
    public Vector3 jumpLocation;
    public int fuelCost;
    public bool jumping;
    public Text shipsJumpedText;
    public List<GameObject> shipsInFleet = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (jumping == true)
        {
            shipsJumpedText.text = shipsJumped.ToString() + "/" + shipsInFleetCount.ToString() + " Currently jumped away.";
            if (shipsJumped >= shipsInFleetCount) { jumping = false; shipsJumped = 0; shipsJumpedText.text = "Fleet Away"; }
        }

    }
    [PunRPC]
    public void Jumping(int jumpCoords) {
        jumpCoordinates = jumpCoords;
        jumpDestination = GameObject.Find(jumpCoordinates.ToString());
        shipsJumped = 0;
        jumping = true;
        for (var i = shipsInFleet.Count - 1; i > -1; i--)
        {

            if (jumpDestination != null)
            {
                if (shipsInFleet[i] != null)
                {
                    //shipsInFleet[i].GetComponent<FleetShip>().jumpOrdered = true;
                    shipsInFleet[i].GetComponent<FleetShip>().OrderJump(jumpCoordinates);
                    //  shipsInFleet[i].GetComponent<FleetShip>().jumpTarget = jumpLocation + (shipsInFleet[i].GetComponent<FleetShip>().myPlaceInTheFleetFormation * 10);
                }
                else { shipsInFleet.RemoveAt(i); }
            }
        }
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
