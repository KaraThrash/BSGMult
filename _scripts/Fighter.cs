using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Photon.PunBehaviour
{
    public int hp;
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
        GetComponent<Rigidbody>().isKinematic = false;
        //transform.parent = null; //TODO: Do I need this? the locationchange/hangar exits should handle this.
        if (pilot != null)
        { pilot.GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer); }


        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().AddForce(transform.up * 10, ForceMode.Impulse);
        this.photonView.ownerId = photonPlayerNumber;
        flying = true;


    }

    [PunRPC]
    public void Land(string myHangar) //Used by players/pilots
    {
        
            playerControlled = false;
            myModel.active = true;
            myCam.active = false;
            exit = false;
            this.photonView.ownerId = 0;
            flying = false;
            currentHangar = GameObject.Find(myHangar);
            if (currentHangar != null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                currentHangar.GetComponent<LandingBay>().AddThisShip(this.gameObject);
            }
            else { }

            if (pilot != null)
            {
                pilot.transform.position = cockpitEntrance.transform.position;
                pilot.active = true;
                pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
                pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer);
            }
            pilot = null;
        

    }
    public void OnTriggerEnter(Collider col3)
    {
        if (col3.gameObject.tag == "HangarSpot") { col3.transform.name = "taken"; }
        if (photonView.isMine == true)
        {
            if (col3.gameObject.tag == "ShipEntrance")
            {
                if (col3.GetComponent<LocationChange>().forFighters == true)
                {
                    if (col3.gameObject.GetComponent<LocationChange>().exit != null) {
                        transform.position = col3.gameObject.GetComponent<LocationChange>().exit.transform.position;
                        transform.rotation = col3.gameObject.GetComponent<LocationChange>().exit.transform.rotation;
                    }
                    
                    if (col3.GetComponent<LocationChange>().enter == false)
                    {
                        if (currentHangar != null) { col3.gameObject.GetComponent<LocationChange>().myHangar.GetComponent<LandingBay>().ShipLeavesHangar(this.gameObject); }
                        
                        GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllBufferedViaServer);
                    }
                }

            }
        }

    }
    public void OnTriggerExit(Collider col)
    {

        if (col.gameObject.tag == "HangarSpot") { col.transform.name = "open"; }
        if (col.gameObject.tag == "Border" && m_PhotonView.isMine == true)
        {
            //TODO: give the border an object that looks at the ship and then moves it. That way you can't fly backward out of the border and end up going out the opposite one.
            Debug.Log("border");
            //transform.position = pacManObject.transform.position;
        }
    }


    //TODO: way back into the ship without jumping? someone needs to retract the pods?
    [PunRPC]
    public void LandOnDockingBay() //used by landingbay scripts
    {
        if (flying == false) // To make sure there is no conflict with players joining the server
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            playerControlled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            myCam.active = false;
            myModel.active = true;
            this.photonView.ownerId = 0;
            flying = false;
       
            if (currentHangar != null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = currentHangar.transform;
                GetComponent<PhotonView>().RPC("SetHangar", PhotonTargets.AllBufferedViaServer, currentHangar.transform.name);
                GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllBufferedViaServer);
            }
            if (pilot != null)
            {
                pilot.transform.position = cockpitEntrance.transform.position;

                //pilot.active = true;
                pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
                pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer);
            }
        }
    }
    public void OnCollisionExit(Collision col3)
    {
        if (col3.gameObject.tag == "LandingPad" && photonView.isMine == true)
        {
            currentHangar = null;
        }
    }
    public void OnCollisionEnter(Collision col2)
    {

        if (col2.gameObject.tag == "LandingPad" && photonView.isMine == true)
        {
            
            GetComponent<PhotonView>().RPC("SetHangar", PhotonTargets.AllViaServer, col2.transform.name);
        }
        if (photonView.isMine == true)
        {


            if (col2.gameObject.tag == "Bullet" && playerControlled == true)
            {
                GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);

            }
        }
    }
    [PunRPC]
    public void SetHangar(string hangarFromCollision) { currentHangar = GameObject.Find(hangarFromCollision); }

    [PunRPC]
    public void ParentToShip()
    {
        if (currentHangar != null)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = currentHangar.transform;
        }

    }
    [PunRPC]
    public void NoParent() { transform.parent = null; currentHangar = null; }
    [PunRPC]
    public void TakeDamage()
    {
        //TODO: is that counting once for everyone in the server? need to check this
        hp--;
        if (hp == 0)
        {
            if (pilot != null)
            {
                pilot.transform.position = medbay.transform.position;

                pilot.active = true;
                pilot.GetComponent<PlayerCharacter>().myCamera.active = true;

            }
            GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllViaServer);

        }
    }
    [PunRPC]
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
        Destroy(this.gameObject);

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(flying);
           // stream.SendNext(transform.position);
        }
        else
        {
            flying = (bool)stream.ReceiveNext();
           // transform.position = (Vector3)stream.ReceiveNext();
        }
    }
    public void Interact(GameObject whoUsedMe)
    {
        //when interacted with it takes off
        if (flying == false)
        {
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
            GetComponent<PhotonView>().RPC("TakeOff", PhotonTargets.AllBufferedViaServer, whoUsedMe.GetComponent<PhotonView>().owner.ID);
        }
    }
}