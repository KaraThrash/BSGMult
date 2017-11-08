using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Photon.PunBehaviour
{
    public int hp;
    public GameObject myColliders;
    public GameObject pilot;
    public bool flying;
    PhotonView m_PhotonView;
    private Rigidbody rb;
    public GameObject cockpitEntrance;
    public bool exit;
    public GameObject myCam;
    public bool playerControlled;
    private GameObject medbay;
    public GameObject currentHangar;
    public GameObject myModel;
    public string medbayName;
    public bool xwing;
    public int currentCords;
    public GameObject jumpManager;
    public GameObject spaceObject;
    public GameObject masterShipList;
    public GameObject landingGear;

    public GameObject explosion;
    public bool destroyed;
    public float dieClock;
    public int myShipGroup; //1: galactica + active fleet 2: Cylon fleet 3: space/planet/leftbehind
    public bool inHangar;
    public GameObject hangarSpace;

    public GameObject dangerText;
    public GameObject criticalFailureText;
    public float colliderTimer;
    public GameObject dradisModel;
    //public GameObject dradisTarget; //turned off when not flying by the landing bay so it isnt picked up on dradis
    // Use this for initialization
    void Start()
    {

        medbay = GameObject.Find(medbayName);
        rb = GetComponent<Rigidbody>();
        m_PhotonView = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (colliderTimer > 0) { colliderTimer -= Time.deltaTime; }
        if (colliderTimer <= 0 && myColliders != null) { myColliders.active = true; }
        if (destroyed == true)
        {
            dieClock -= Time.deltaTime;
            if (dieClock <= 0) { Die(); }
        }
        if (flying == true)
        {

        }
        else { }
  

        if (m_PhotonView.isMine == true && flying == true)
        {
           
        }
    }


    [PunRPC]
    public void TakeOff(int photonPlayerNumber)
    {
        landingGear.active = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = false;
        if (inHangar == true)
        {


            myColliders.active = false;
            colliderTimer = 2.0f;
            hangarSpace.GetComponent<DockingSpace>().spaceOpen = true;
            currentHangar.GetComponent<LandingBay>().ShipLeavesHangar(this.gameObject);
            if (photonView.isMine == false)
            {
                this.gameObject.active = currentHangar.GetComponent<LandingBay>().myShip.active;


            }
            GetComponent<PhotonView>().RPC("OutInSpace", PhotonTargets.AllBufferedViaServer, currentCords);
           
            transform.position = hangarSpace.GetComponent<DockingSpace>().myLaunchBay.transform.position;
            transform.rotation = hangarSpace.GetComponent<DockingSpace>().myLaunchBay.transform.rotation;
            GetComponent<Rigidbody>().AddForce(transform.forward * -330, ForceMode.Impulse);
        }
        else { GetComponent<Rigidbody>().AddForce(transform.up * 10, ForceMode.Impulse); }
        hangarSpace = null;
        inHangar = false;
        flying = true;
        this.photonView.ownerId = photonPlayerNumber;
        


    }

    //[PunRPC]

    public void Land() //Used by players/pilots
    {
        GetComponent<Rigidbody>().isKinematic = true;
        playerControlled = false;
            myModel.active = true;
            myCam.active = false;
            exit = false;
        
        flying = false;
        if (currentHangar != null)
        {
            
            GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllBufferedViaServer, currentHangar.GetComponent<LandingBay>().myShipInList, currentHangar.GetComponent<LandingBay>().myShip.GetComponent<FTLDrive>().currentCords);

            // currentHangar.GetComponent<LandingBay>().AddThisShip(this.gameObject);


        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
            if (pilot != null)
            {
                pilot.transform.position = cockpitEntrance.transform.position;
                pilot.active = true;
                pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
            }
            pilot = null;
        }
            
            
               
            
            
        GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);

    }
    public void OnTriggerEnter(Collider col3)
    {
        if (col3.gameObject.tag == "ExitLaunchBay") { myColliders.active = true; flying = true; }
        if (photonView.isMine == true)
        {
        }

    }
    public void OnTriggerExit(Collider col)
    {

        //if (col.gameObject.tag == "HangarSpot") { col.transform.name = "open"; }
        if (col.gameObject.tag == "Border" && m_PhotonView.isMine == true)
        {
            //TODO: give the border an object that looks at the ship and then moves it. That way you can't fly backward out of the border and end up going out the opposite one.
            Debug.Log("border");
            //transform.position = pacManObject.transform.position;
        }
    }


    //TODO: way back into the ship without jumping? someone needs to retract the pods?
    [PunRPC]
    public void LandOnDockingBay(int dockingBayCords, int spotInBay ,int dockingBayFromList) //used by landingbay scripts
    {
      //  if (flying == false) // To make sure there is no conflict with players joining the server
       // {
            //GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient);
            this.gameObject.active = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            playerControlled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            myCam.active = false;
            myModel.active = true;
            //this.photonView.ownerId = 0;
            flying = false;
           
            GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllBufferedViaServer, dockingBayFromList, dockingBayCords);
            GetComponent<Rigidbody>().isKinematic = true;
            if (currentHangar != null)
            {
                
                if (currentHangar.GetComponent<LandingBay>().shipSpots[spotInBay] != null)
                {
                    transform.position = currentHangar.GetComponent<LandingBay>().shipSpots[spotInBay].transform.position;
                    transform.rotation = currentHangar.GetComponent<LandingBay>().shipSpots[spotInBay].transform.rotation;
                inHangar = true;
                }
                // transform.parent = currentHangar.transform;
                
               
            }
            if (pilot != null)
            {
                pilot.transform.position = cockpitEntrance.transform.position;
                pilot.GetComponent<PlayerCharacter>().JumpEffects(dockingBayCords, myShipGroup);
           // pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer);
            }
       // }
    }
    public void OnCollisionExit(Collision col3)
    {
        if (col3.gameObject.tag == "LandingPad" && photonView.isMine == true)
        {
            //flying = true;
            //GetComponent<PhotonView>().RPC("OutInSpace", PhotonTargets.AllBufferedViaServer,currentCords);
            currentHangar = null;
        }
    }
    public void OnCollisionEnter(Collision col2)
    {

        if (col2.gameObject.tag == "LandingPad" && photonView.isMine == true)
        {
            //flying = false;
            GetComponent<PhotonView>().RPC("SetHangar", PhotonTargets.AllViaServer, col2.gameObject.GetComponent<LandingBay>().myShipInList);
        }
        if (photonView.isMine == true && destroyed == false)
        {


            if (col2.gameObject.tag == "Bullet" && playerControlled == true)
            {
                GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);

            }
        }
    }
    [PunRPC]
    public void NoHangar()
    { currentHangar = null; }
    [PunRPC]
    public void SetHangar(int hangarToSet) {
        
        currentHangar = masterShipList.GetComponent<MasterShipList>().ParentFighterToShip(hangarToSet);
    }
    [PunRPC]
    public void OutInSpace(int spaceCords) {
        transform.parent = spaceObject.transform;
        currentHangar = null;
        //if (photonView.isMine == false)
        //{
        //    if (jumpManager.GetComponent<JumpManager>().CheckCoordinates(spaceCords) != true)
        //    {
        //        this.gameObject.active = false;

        //    }
        //    else
        //    {
        //        this.gameObject.active = true;

        //    }
        //}
    }
    [PunRPC]
    public void ParentToShip(int shipFromMasterList,int cordsOfParent)
    {
        currentHangar = masterShipList.GetComponent<MasterShipList>().ParentFighterToShip(shipFromMasterList);
        transform.parent = masterShipList.GetComponent<MasterShipList>().ParentFighterToShip(shipFromMasterList).transform;
        GetComponent<FTLDrive>().currentCords = cordsOfParent;
        currentCords = cordsOfParent;
        if (currentHangar != null)
        {
            currentHangar.GetComponent<LandingBay>().AddThisShip(this.gameObject);

        
           // GetComponent<Rigidbody>().isKinematic = true;
          //  transform.parent = currentHangar.transform;

        }

    }
    [PunRPC]
    public void NoParent() { transform.parent = null; currentHangar = null; }
    [PunRPC]
    public void TakeDamage()
    {
        //TODO: is that counting once for everyone in the server? need to check this
        if (destroyed == false) {
            hp--;
            if (hp <= 0 )
            {
                destroyed = true;
                dieClock = 4;
                if (pilot != null)
                {
                    dangerText.active = false;
                    criticalFailureText.active = true;
                }


            }
            else { dangerText.active = true; }
        }
    }
    public void DieOnServer() {
     // GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllViaServer);
    }
   // [PunRPC]

    public void Die()
    {

        if (pilot != null)
        {
            pilot.transform.parent = medbay.transform;
            pilot.transform.position = medbay.transform.position;
            pilot.transform.rotation = medbay.transform.rotation;

            pilot.active = true;
            pilot.GetComponent<PlayerCharacter>().myCamera.active = true;

        }
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
            stream.SendNext(flying);
        }
        else
        {
            transform.rotation = (Quaternion)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
            flying = (bool)stream.ReceiveNext();
        }
    }
    public void Interact(GameObject whoUsedMe)
    {
        //when interacted with it takes off
        GetComponent<PhotonView>().RequestOwnership();
        if (xwing == true)
            {
                //whoUsedMe.GetComponent<HumanControls>().cam.GetComponent<FPScamera>().lockCursor = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.lockState = CursorLockMode.Confined;
            }

            playerControlled = true;
            pilot = whoUsedMe;

            whoUsedMe.active = false;
            myCam.active = true;
            myModel.active = false;

            pilot.GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer);
           // GetComponent<PhotonView>().RPC("TakeOff", PhotonTargets.AllBufferedViaServer);
        GetComponent<PhotonView>().RPC("TakeOff", PhotonTargets.AllViaServer, whoUsedMe.GetComponent<PhotonView>().owner.ID);
    }
}