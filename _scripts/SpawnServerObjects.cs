using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnServerObjects : MonoBehaviour {
    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;
    public GameObject spot4;
    public GameObject galacticaSpot;
    public GameObject basestarSpot;
    public GameObject humanShipInterior;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SpawnEverythingOnServer() {
        //PhotonNetwork.InstantiateSceneObject("Galactica", new Vector3(222,0,150), new Quaternion(0, 0, 0, 0), 0, null);
       // PhotonNetwork.InstantiateSceneObject("UnknownSpaceLocation", new Vector3(50,50,50), new Quaternion(0, 0, 0, 0), 0, null);
        GameObject clone = PhotonNetwork.InstantiateSceneObject("Galactica", galacticaSpot.transform.position, new Quaternion(0, 0, 0, 0), 0, null) as GameObject;
        clone.GetComponent<Galactica>().myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
        PhotonNetwork.InstantiateSceneObject("BaseStar", basestarSpot.transform.position, new Quaternion(0, 0, 0, 0), 0, null);
        PhotonNetwork.InstantiateSceneObject("scorekeeper", humanShipInterior.transform.position, new Quaternion(0, 0, 0, 0), 0, null);
       // GameObject.Find("Galactica(CLone)") GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
        //PhotonNetwork.InstantiateSceneObject("viper", spot1.transform.position, new Quaternion(0, 0, 0, 0), 0, null);
        //PhotonNetwork.InstantiateSceneObject("viper", spot2.transform.position, new Quaternion(0, 0, 0, 0), 0, null);
        //PhotonNetwork.InstantiateSceneObject("xwing", spot3.transform.position, new Quaternion(0, 0, 0, 0), 0, null);
        //PhotonNetwork.InstantiateSceneObject("xwing", spot4.transform.position, new Quaternion(0, 0, 0, 0), 0, null);


    }
}
