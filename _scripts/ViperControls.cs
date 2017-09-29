using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViperControls : Photon.PunBehaviour
{
    public int hp;
    public GameObject scoreKeeper;
    public GameObject pacManObject;
    public GameObject pilot;
    public bool flying;
    PhotonView m_PhotonView;
    private Rigidbody rb;
    public GameObject cockpitEntrance;
    public float lift;
    public bool exit;
    public float hort;
    public float vert;
    public float roll;
    public float mouseX;
    public float mouseY;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject bullet;
    public GameObject myCam;
    public float gunCoolDown;
    public bool xwing;
    public bool playerControlled;
    public float afterBurner;
    private GameObject medbay;
    private Vector3 holdPosValue;
    public GameObject currentHangar;
    public GameObject myModel;
    // Use this for initialization
    void Start () {
       
        medbay = GameObject.Find("medbay");
        rb = GetComponent<Rigidbody>();
        m_PhotonView = GetComponent<PhotonView>();
        scoreKeeper = GameObject.Find("scorekeeper(Clone)");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flying == true)
        {

        }
        else {  }
        if (gunCoolDown > 0) { gunCoolDown -= Time.deltaTime; }

        if (m_PhotonView.isMine == true && flying == true)
        {
            if (xwing == true) { MouseFlightControls(); } else { KeyboardFlightControls(); }
        }
        else { hort = 0; vert = 0; }
    }

   public void  MouseFlightControls()
    {
        if (Input.GetMouseButton(0))
        {
            if (gunCoolDown <= 0)
            {
                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                gunCoolDown = 0.2f;
            }
            gunCoolDown -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.T) && currentHangar != null) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.AllBufferedViaServer, currentHangar.transform.name); }
        if (Input.GetKey(KeyCode.Space)) { lift = 3; } else if (Input.GetKey(KeyCode.LeftShift)) { lift = -3; } else { lift = 0; }
        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Q)) { roll = -60; } else if (Input.GetKey(KeyCode.E)) { roll = 60; } else { roll = 0; }
        if (Input.mousePosition.x < 600) { mouseX = Input.mousePosition.x - 600; } else if (Input.mousePosition.x > 700) { mouseX = Input.mousePosition.x - 700; } else { mouseX = 0; }
        if (Input.mousePosition.y < 325) { mouseY = Input.mousePosition.y - 325; } else if (Input.mousePosition.y > 475) { mouseY = Input.mousePosition.y - 475; } else { mouseY = 0; }
        //GetComponent<PhotonView>().RPC("flightControls", PhotonTargets.AllViaServer, vert, hort, roll, (mouseX * 0.5f), (-mouseY * 0.5f), exit, lift);
        flightControls(vert, hort, roll, mouseX, -mouseY, exit, lift);
    }
    
    public void KeyboardFlightControls()
    {
        if (Input.GetKey(KeyCode.Space)) {
            if (gunCoolDown <= 0)
            {
                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                gunCoolDown = 0.2f;
            }
            gunCoolDown -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.T) && currentHangar != null) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.AllBufferedViaServer, currentHangar.transform.name); }
        if (Input.GetKey(KeyCode.KeypadPlus)) { lift = 5; } else if (Input.GetKey(KeyCode.KeypadEnter)) { lift = -5; } else { lift = 0; }

        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        // if (Input.GetKey(KeyCode.KeypadPlus)) { vert += Time.deltaTime; }
        //if (Input.GetKey(KeyCode.KeypadMinus)) { vert -= Time.deltaTime; }
        if (Input.GetKey(KeyCode.Keypad7)) { roll = -120; } else if (Input.GetKey(KeyCode.Keypad9)) { roll = 120; } else { roll = 0; }
        if (Input.GetKey(KeyCode.Keypad4)) { mouseX = -160; } else if (Input.GetKey(KeyCode.Keypad6)) { mouseX = 160; } else { mouseX = 0; }
        if (Input.GetKey(KeyCode.Keypad2)) { mouseY = -160; } else if (Input.GetKey(KeyCode.Keypad8)) { mouseY = 160; } else { mouseY = 0; }
        if (Input.GetKey(KeyCode.Keypad5)) { afterBurner = 1400.0f; }
        if (afterBurner > 0) { afterBurner -= 4; }

        //GetComponent<PhotonView>().RPC("flightControls", PhotonTargets.AllViaServer, vert, hort, roll, mouseX, mouseY, exit, lift);
        flightControls(vert, hort, roll, mouseX, mouseY, exit, lift);
    }
    [PunRPC]
    public void ShootGuns()
    {
            Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
            Instantiate(bullet, gun1.transform.position, gun1.transform.rotation);
            
    }

    [PunRPC]
    public void TakeOff(int photonPlayerNumber) {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null;

      //  if (flying == false) {
            

            if (pilot != null)
            { pilot.GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer); }
        

            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().AddForce(transform.up  * 10,ForceMode.Impulse);
            this.photonView.ownerId = photonPlayerNumber;
            // GameObject.Find(player).transform.parent = this.transform;
             flying = true;
           
            
       // } else { }
        
    }

    [PunRPC]
    public void Land(string myHangar)
    {

        // if (flying == true)
        // {
        
        playerControlled = false;
        myModel.GetComponent<MeshRenderer>().enabled = true;
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
        if (pilot != null)
        {
            pilot.transform.position = cockpitEntrance.transform.position;
            pilot.active = true;
            pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
            pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer);
        }
        pilot = null;
        //GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllViaServer); 

        // }


    }
    public void OnTriggerEnter(Collider col3)
    {
        if (col3.gameObject.tag == "HangarSpot") { col3.transform.name = "taken"; }
        if (photonView.isMine == true)
        {
            if (col3.gameObject.tag == "ShipEntrance")
            {
                //if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true) { transform.parent = col.GetComponent<LocationChange>().myParent.transform; }
                if (col3.GetComponent<LocationChange>().forFighters == true)
                {
                    transform.position = col3.gameObject.GetComponent<LocationChange>().exit.transform.position;
                    transform.rotation = col3.gameObject.GetComponent<LocationChange>().exit.transform.rotation;
                    if (col3.GetComponent<LocationChange>().enter == false) { col3.gameObject.GetComponent<LocationChange>().myHangar.GetComponent<LandingBay>().ShipLeavesHangar(this.gameObject);
                        GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllBufferedViaServer);
                    }
                }
                
            }
        }

    }
    public void OnTriggerExit(Collider col)
    {
        
        if (col.gameObject.tag == "HangarSpot") { col.transform.name = "open"; }
        if (col.gameObject.tag == "Border" &&  m_PhotonView.isMine == true)
        {
            //TODO: give the border an object that looks at the ship and then moves it. That way you can't fly backward out of the border and end up going out the opposite one.
            Debug.Log("border");
            //transform.position = pacManObject.transform.position;
        }
    }
   // [PunRPC]
    public void flightControls(float vert,float hort,float roll,float rollX,float rollY,bool leave, float lift) {
        if (vert != 0)
        {
            rb.AddForce(transform.forward * ((-vert * 300) - afterBurner)) ;
        
        }
        if (hort != 0)
        {
            rb.AddForce(transform.right * (-hort * 5), ForceMode.Impulse);
        }
        //TODO: two monitors makes the X value freak out
        Mathf.Clamp(rollX, -50.0F, 50.0F);
        Mathf.Clamp(rollY, -50.0F, 50.0F);
        if (roll != 0) { rb.AddTorque(transform.forward * roll * Time.deltaTime,ForceMode.Impulse); }
        if (rollX != 0) { rb.AddTorque(transform.up *  rollX * Time.deltaTime, ForceMode.Impulse); }
        if (rollY != 0) { rb.AddTorque(transform.right *  -rollY * Time.deltaTime, ForceMode.Impulse); }
        if (lift != 0) { rb.AddForce(transform.up * lift * 30); }
       // if (leave == true &&  GetComponent<Rigidbody>().velocity.magnitude < 30) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.All); }

    }

    //TODO: way back into the ship without jumping? someone needs to retract the pods?
    [PunRPC]
    public void LandOnDockingBay() {
        if (flying == false)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            playerControlled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            myCam.active = false;
            myModel.GetComponent<MeshRenderer>().enabled = true;
            //GetComponent<Rigidbody>().useGravity = true;
            this.photonView.ownerId = 0;
            flying = false;
            if (pilot != null)
            {
                pilot.transform.position = cockpitEntrance.transform.position;

                //pilot.active = true;
                pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
                pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer);
            }
            pilot = null;
            if (currentHangar != null)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = currentHangar.transform;
                GetComponent<PhotonView>().RPC("SetHangar", PhotonTargets.AllBufferedViaServer, currentHangar.transform.name);
                GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllBufferedViaServer);
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
            //currentHangar = col2.transform.name;
            Debug.Log(col2.transform.name);
            GetComponent<PhotonView>().RPC("SetHangar", PhotonTargets.AllViaServer, col2.transform.name);
        }
        if (photonView.isMine == true)
        {
            

            if (col2.gameObject.tag == "Bullet" && playerControlled == true)
            {
                GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);

                scoreKeeper = GameObject.Find("scorekeeper(Clone)");
                scoreKeeper.GetComponent<ScoreKeeper>().ScoreAHit(col2.gameObject.GetComponent<Bullet>().owner);
                //scoreKeeper.GetComponent<PhotonView>().RPC("Score", PhotonTargets.AllBufferedViaServer, col2.gameObject.GetComponent<Bullet>().owner);
                //Destroy(col2.gameObject);
            }
        }
    }
    [PunRPC]
    public void SetHangar(string hangarFromCollision) { currentHangar = GameObject.Find(hangarFromCollision); }

    [PunRPC]
    public void ParentToShip() {
        if (currentHangar != null) {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = currentHangar.transform;
        }
        
    }
    [PunRPC]
    public void NoParent() { transform.parent = null; currentHangar = null; }
    [PunRPC]
    public void TakeDamage() {
        hp--;
        if (hp <= 0) { if (pilot != null) {
                pilot.transform.position = medbay.transform.position;
                
                pilot.active = true;
                pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
                
            }
            Destroy(this.gameObject);
        } 
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(flying);
            stream.SendNext(transform.position);
        }
        else
        {
            flying = (bool)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
    public void Interact(GameObject whoUsedMe) {
        //when interacted with it takes off
        if (flying == false)
        {
            if (xwing == true) {
                //whoUsedMe.GetComponent<HumanControls>().cam.GetComponent<FPScamera>().lockCursor = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.lockState = CursorLockMode.Confined;
            }

            playerControlled = true;
            pilot = whoUsedMe;
            
            whoUsedMe.active = false;
            myCam.active = true;
            myModel.GetComponent<MeshRenderer>().enabled = false;
            GetComponent<PhotonView>().RPC("TakeOff", PhotonTargets.AllBufferedViaServer, whoUsedMe.GetComponent<PhotonView>().owner.ID);
        }
    }
}
