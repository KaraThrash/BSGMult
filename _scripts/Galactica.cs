using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Galactica : Photon.PunBehaviour
{
    public int currentCord;
    public GameObject baseStar;
    public Transform peopleOnBoard;
    public GameObject jumpManager;
    public GameObject shipInterior;
    public GameObject myHangar;
    public GameObject localSpacePositionObject;//use this to set duplicate local position for jump position
    public GameObject theGalactica;
    public GameObject theFleet;
    public GameObject theActiveFleet;
    public float speed;
    public bool manned;
    public GameObject myCamera;
    public GameObject fwdObject;
    public GameObject rotationObject;
    public int fuel;
    private int jumpSpotTest = 0;
    public GameObject myFtlComputer;
    public GameObject medbay;
    public GameObject dradisModel;
    public GameObject engineAnim;
    void Start () {

    }
    void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
           

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
           

        }
        if (photonView.isMine == true)
        {
            
           
        }

        theGalactica.transform.position = Vector3.MoveTowards(theGalactica.transform.position, fwdObject.transform.position, 3.0f * Time.deltaTime);
        theGalactica.transform.rotation = Quaternion.Slerp(theGalactica.transform.rotation, rotationObject.transform.rotation, 1.0f * Time.deltaTime);
        fwdObject.transform.position = Vector3.MoveTowards(fwdObject.transform.position, theGalactica.transform.position, 3.0f );
        if (Vector3.Distance(fwdObject.transform.position, theGalactica.transform.position) > 10) { engineAnim.active = true; } else { engineAnim.active = false; }
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
            GetComponent<PhotonView>().RPC("SetRotationObjects", PhotonTargets.Others, rotationObject.transform.rotation);
        }
    }
    [PunRPC]
    public void SetNewCords(int newCord) { currentCord = newCord; GetComponent<FTLDrive>().currentCords = newCord; }



    [PunRPC]
    public void SetRotationObjects(Quaternion rot) { rotationObject.transform.rotation = rot; }
    [PunRPC]
    public void SetForwardObject() { fwdObject.transform.localPosition = new Vector3( 0, 0, -1000.0f); }

    public void Manned() { rotationObject.transform.rotation = theGalactica.transform.rotation; manned = true; myCamera.GetComponent<RPGCamera>().Target = theGalactica.transform; myCamera.active = true; }
    public void NotManned() { manned = false; myCamera.active = false; rotationObject.transform.rotation = theGalactica.transform.rotation; }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(transform.rotation);
            //stream.SendNext(transform.position);
            stream.SendNext(currentCord);
        }
        else
        {
           // transform.rotation = (Quaternion)stream.ReceiveNext();
           // transform.position = (Vector3)stream.ReceiveNext();
            currentCord = (int)stream.ReceiveNext();
        }
    }
    //[PunRPC]
    //public void Jump(int coordsToJump) {
    //    fuel -= 1;
 
    //    currentCord = coordsToJump;
    //    GetComponent<FTLDrive>().currentCords = coordsToJump;
    //    baseStar.GetComponent<PhotonView>().RPC("StartFTL", PhotonTargets.AllViaServer, coordsToJump);
    //    GetComponent<PhotonView>().RPC("SetNewCords", PhotonTargets.AllBufferedViaServer, coordsToJump);
    //    jumpManager.GetComponent<PhotonView>().RPC("UpdateLocationGalactica", PhotonTargets.AllBufferedViaServer, coordsToJump);
    //    //GameObject.Find("BaseStar(Clone)").GetComponent<BaseStar>().StartFTL();
    //    ForPassengersDuringJump(coordsToJump);
    //    myHangar.GetComponent<LandingBay>().jumping = true;



    //}
    //public void ForPassengersDuringJump(int newCords)
    //{
    //    foreach (Transform child in peopleOnBoard)
    //    {
    //        if (child.GetComponent<PlayerCharacter>().localPlayer != null)
    //        {
    //            //TODO: what about people sitting on the flight deck? >> handled on fighter script currently
    //            if (child.GetComponent<PlayerCharacter>().flying == false)
    //            {

    //                child.GetComponent<PlayerCharacter>().localPlayer.GetComponent<PlayerMain>().spaceCoordinates = newCords;
    //                child.GetComponent<PlayerCharacter>().JumpEffects(newCords);
    //                jumpManager.GetComponent<JumpManager>().ManageJump(newCords, 0, 0, newCords); //galactica cords, fleet cords, basestar cords, localPlayer cords

    //            }
    //            else { jumpManager.GetComponent<JumpManager>().ManageJump(newCords, 0, 0, 0); }

    //        }



    //    }

    //}
}
