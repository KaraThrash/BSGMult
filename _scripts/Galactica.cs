using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galactica : Photon.PunBehaviour
{
    public Transform peopleOnBoard;
    public GameObject shipInterior;
    public GameObject myHangar;
    public GameObject localSpacePositionObject;//use this to set duplicate local position for jump position
    public GameObject theGalactica;
    public GameObject theFleet;
    public float speed;
    public bool manned;
    public GameObject myCamera;
    public GameObject fwdObject;
    public GameObject rotationObject;
    public int fuel;
    private int jumpSpotTest = 0;
    public GameObject myFtlComputer;
    // Use this for initialization
    void Start () {
       // myCamera.transform.parent = null;
       // rotationObject.transform.parent = null;
        theFleet.transform.parent = null;
        //GameObject spawnLocation = GameObject.Find("PlayerSpawn");
        //spawnLocation.transform.parent = shipInterior.transform;
    }

    // Update is called once per frame
    void Update()
    {

        theGalactica.transform.rotation = Quaternion.Lerp(transform.rotation, rotationObject.transform.rotation, 5.0f);
        theGalactica.transform.position = Vector3.MoveTowards(theGalactica.transform.position,fwdObject.transform.position, 1.0f);
        fwdObject.transform.position = Vector3.MoveTowards(fwdObject.transform.position, theGalactica.transform.position, 2.0f);
        if (manned == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GetComponent<PhotonView>().RPC("SetForwardObject", PhotonTargets.AllViaServer);
                //transform.Translate(Vector3.right * 1);
            }
            
            if (Input.GetKey(KeyCode.S)) { rotationObject.transform.Rotate(0.5f,0, 0); }
            if (Input.GetKey(KeyCode.W)) { rotationObject.transform.Rotate(-0.5f,0, 0); }
            if (Input.GetKey(KeyCode.Q)) { rotationObject.transform.Rotate(0, 0,-0.5f); }
            if (Input.GetKey(KeyCode.E)) { rotationObject.transform.Rotate(0, 0,0.5f); }
            if (Input.GetKey(KeyCode.D)) { rotationObject.transform.Rotate(0, 0.5f, 0); }
            if (Input.GetKey(KeyCode.A)) { rotationObject.transform.Rotate(0, -0.5f, 0); }
            GetComponent<PhotonView>().RPC("SetMovementObjects", PhotonTargets.Others, rotationObject.transform.rotation, fwdObject.transform.localPosition);
        }
    }

    [PunRPC]
    public void SetMovementObjects(Quaternion rot, Vector3 pos) { fwdObject.transform.localPosition = pos; rotationObject.transform.rotation = rot; }
    [PunRPC]
    public void SetForwardObject() { fwdObject.transform.localPosition = new Vector3( 0, 0, -1000.0f); }

    public void Manned() { rotationObject.transform.rotation = theGalactica.transform.rotation;  manned = true; myCamera.active = true; }
    public void NotManned() { manned = false; myCamera.active = false; rotationObject.transform.rotation = theGalactica.transform.rotation; }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           
          //  stream.SendNext(fwdObject.transform.position);
          //  stream.SendNext(rotationObject.transform.rotation);
        }
        else
        {
           // fwdObject.transform.position = (Vector3)stream.ReceiveNext();
           // rotationObject.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Jump(int coordsToJump) {
        fuel -= 1;
        Debug.Log(coordsToJump);
        if (GameObject.Find(coordsToJump.ToString()) != null)
        {
            GameObject.Find("BaseStar(Clone)").GetComponent<PhotonView>().RPC("StartFTL", PhotonTargets.AllBufferedViaServer, coordsToJump);
            //GameObject.Find("BaseStar(Clone)").GetComponent<BaseStar>().StartFTL();
            ForPassengersDuringJump();
            myHangar.GetComponent<LandingBay>().jumping = true;
            // theGalactica.transform.position = Vector3.zero;
            transform.position = GameObject.Find(coordsToJump.ToString()).transform.position;
           // myHangar.GetComponent<LandingBay>().jumping = false;
            // myHangar.GetComponent<LandingBay>().JumpNotFromServer();
        }

    }
    public void ForPassengersDuringJump() {
        foreach (Transform child in peopleOnBoard)
            child.GetComponent<PlayerCharacter>().JumpEffects();
    }
}
