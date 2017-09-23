using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FTLTarget : Photon.PunBehaviour
{
    public string locationType;
    //space sector coordinates
    public int xCord;
    public int yCord;
    public int zCord;
    public bool mapped;
    //TODO: have a mapped bool for humans and cylons
   // public bool mappedForCylons;
    public int fuelCost;
    public int distance;
    public int risk;
    public bool isEarth;
    public bool canJumpTo;
    public bool isSet;
    public Text myText;
    public GameObject locationInSpace;
   // public GameObject unknownLocations;
    public GameObject unknownLocation;
    //public GameObject mappedLocations;
    public bool objectsSpawned;

    // Use this for initialization
    void Start () {
      //  GetComponent<PhotonView>().ObservedComponents = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    [PunRPC]
    public void Map()
    {
        mapped = true;

    }

    [PunRPC]
    public void SetFtl(int fuel, int dist,int thisRisk) {
       // fuelCost = fuel; distance = dist; risk = thisRisk; isSet = true;
       // if (myText != null) { myText.text = distance.ToString() + " Units costing " + fuelCost.ToString() + " Fuel at a risk of " + risk.ToString(); }
        

    }
    public void ResetFtl()
    {

       // fuelCost = 0; distance = 0; risk = 0; isSet = false;
       // locationInSpace = null;
       // myText.text = "";
    }
    //[PunRPC]
    //public void SpawnSpace()
    //{
    //    if (locationInSpace == null )
    //    {
    //        if (risk == 1)
    //        {
    //            GameObject clone = PhotonNetwork.Instantiate("SpaceLocation_planet", unknownLocation.transform.position, new Quaternion(0, 0, 0, 0), 0, null) as GameObject;
    //            locationInSpace = clone;
    //            clone.GetComponent<FTLTarget>().SetFtl(fuelCost,distance,risk);
    //            //clone.transform.parent = transform.parent.GetComponent<FTLcomputer>().mappedLocations.transform;
    //        }
    //        else
    //        {
    //            GameObject clone = PhotonNetwork.Instantiate("SpaceLocation_Discs", unknownLocation.transform.position, new Quaternion(0, 0, 0, 0), 0, null) as GameObject;
    //            locationInSpace = clone;
    //            clone.GetComponent<FTLTarget>().SetFtl(fuelCost, distance, risk);
    //            //clone.transform.parent = transform.parent.GetComponent<FTLcomputer>().mappedLocations.transform;
    //        }
    //        unknownLocation.transform.position = new Vector3(unknownLocation.transform.position.x + 1000, unknownLocation.transform.position.y + 1000, unknownLocation.transform.position.z);
    //    }
    //}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

           // stream.SendNext(locationInSpace);
            stream.SendNext(mapped);
        }
        else
        {
            // fwdObject.transform.position = (Vector3)stream.ReceiveNext();
            // locationInSpace = (GameObject)stream.ReceiveNext();
            mapped = (bool)stream.ReceiveNext();
        }
    }
}
