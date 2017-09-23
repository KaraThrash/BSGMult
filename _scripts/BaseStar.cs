using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStar : Photon.PunBehaviour
{
    public GameObject launchBay;
    public GameObject raider;
    public GameObject galactica;
    public string objectToSpawn = "Raider";
    public float spawnClock;
    public int ftlCoords;
    public float ftlClock;
    public int numberOfRaiderWings;
    public GameObject raiderParentObjectPrefab;
    public GameObject raiderParentObject;
	// Use this for initialization
	void Start () {
        GameObject clone = Instantiate(raiderParentObjectPrefab, transform.position, transform.rotation) as GameObject;
        raiderParentObject = clone;
    }
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine == true)
        {
            if (spawnClock <= 0 && numberOfRaiderWings > 0)
            {
                GetComponent<PhotonView>().RPC("LaunchRaiders", PhotonTargets.MasterClient);
                numberOfRaiderWings--;
                spawnClock = 3;
            }
            else { spawnClock -= Time.deltaTime; }
            if (ftlClock >= 0)
            {
                ftlClock -= Time.deltaTime;
                if (ftlClock <= 0) { Jump(); }
            }
        }
    }

    [PunRPC]
    void LaunchRaiders()
    {
        
        GameObject clone = PhotonNetwork.InstantiateSceneObject(objectToSpawn, launchBay.transform.position, launchBay.transform.rotation, 0, null);
        clone.transform.parent = raiderParentObject.transform;
    }
    [PunRPC]
    void StartFTL(int coords)
    {
        ftlCoords = coords;
        ftlClock = 10;
    }

    public void Jump() {
    //TODO: modify this so that it is recalling it's fighters and not justs deleteing them

        //Using the parent object was only deleteing on the server
        foreach (Transform child in raiderParentObject.transform)
             {
            PhotonNetwork.Destroy(child.gameObject);
            numberOfRaiderWings++;
        }
        transform.position = GameObject.Find(ftlCoords.ToString()).transform.position;

    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
        else
        {

        }
    }
}
