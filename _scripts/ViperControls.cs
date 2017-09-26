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
            cockpitEntrance.active = false;
            
            // rb.useGravity = false;
        }
        else { cockpitEntrance.active = true; }
        if (gunCoolDown > 0) { gunCoolDown -= Time.deltaTime; }
        // if (this.photonView.ownerId == PhotonNetwork.player.ID)
        //{
        // Debug.Log(Input.mousePosition.x);
        // Debug.Log(Input.mousePosition.y);
        if (Input.GetKeyDown(KeyCode.L)) {  }
        if (Input.GetKeyDown(KeyCode.O)) {  }
        if (m_PhotonView.isMine == true && flying == true)
        {
            
            //MouseFlightControls();
            if (xwing == true) { MouseFlightControls(); } else { KeyboardFlightControls(); }
            
            //flightControls();
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
        if (Input.GetKeyDown(KeyCode.T)) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.All); }
        if (Input.GetKey(KeyCode.Space)) { lift = 2; } else if (Input.GetKey(KeyCode.LeftShift)) { lift = -2; } else { lift = 0; }
        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Q)) { roll = -40; } else if (Input.GetKey(KeyCode.E)) { roll = 40; } else { roll = 0; }
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
        if (Input.GetKey(KeyCode.T)) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.All); }
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
       
        if (flying == false) {
            rb.isKinematic = false;
            //TODO: change this to be controlled by player object
            //switch (photonPlayerNumber)
            //{
            //    case 1:
            //        bullet = bullet1;
            //        break;
            //    case 2:
            //        bullet = bullet2;
            //        break;
            //    case 3:
            //        bullet = bullet3;
            //        break;
            //    case 4:
            //        bullet = bullet4;
            //        break;
            //}
            if (pilot != null)
            { pilot.GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer); }
        
                cockpitEntrance.active = false;
            GetComponent<Rigidbody>().useGravity = false;
            rb.AddForce(transform.up  * 10,ForceMode.Impulse);
            this.photonView.ownerId = photonPlayerNumber;
            // GameObject.Find(player).transform.parent = this.transform;
             flying = true;
           
            
        } else { }
        
    }

    [PunRPC]
    public void Land()
    {

        if (flying == true)
        {
            playerControlled = false;
            myCam.active = false;
            exit = false;
            cockpitEntrance.active = true;
            GetComponent<Rigidbody>().useGravity = true;
            this.photonView.ownerId = 0;
            // GameObject.Find(player).transform.parent = this.transform;
            flying = false;
            if (pilot != null)
            {
                pilot.transform.position = cockpitEntrance.transform.position;
            pilot.active = true;
            pilot.GetComponent<PlayerCharacter>().myCamera.active = true ;
             pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllBufferedViaServer); }
            pilot = null;
        }
        

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
                    if (col3.GetComponent<LocationChange>().enter == false) { col3.gameObject.GetComponent<LocationChange>().myHangar.GetComponent<LandingBay>().ShipLeavesHangar(this.gameObject); }
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
        if (leave == true) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.All); }

    }

    //TODO: way back into the ship without jumping? someone needs to retract the pods?
    [PunRPC]
    public void LandOnDockingBay() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        playerControlled = false;
        rb.isKinematic = false;
        myCam.active = false;
        cockpitEntrance.active = true;
        rb.useGravity = true;
        this.photonView.ownerId = 0;
        flying = false;
        if (pilot != null)
        {
            pilot.transform.position = cockpitEntrance.transform.position;
            
            //pilot.active = true;
            pilot.GetComponent<PlayerCharacter>().myCamera.active = true;
            pilot.GetComponent<PhotonView>().RPC("GetOutShip", PhotonTargets.AllViaServer);
        }
        pilot = null;

    }
    public void OnCollisionExit(Collision col3)
    {
        // holdPosValue = transform.position;

        if (col3.gameObject.tag == "LandingPad")
        {
           // transform.parent = null;
        }
    }
        public void OnCollisionEnter(Collision col2)
    {
        // holdPosValue = transform.position;

        if (col2.gameObject.tag == "LandingPad")
        {
            //transform.parent = col2.transform;
        }
            //    currentHangar = col2.gameObject;
            //    if (photonView.isMine == true) {  GetComponent<PhotonView>().RPC("LandOnDockingBay", PhotonTargets.AllViaServer); }
            //   // this.transform.parent = col2.transform.root;
            //    //GetComponent<PhotonView>().RPC("LandOnDockingBay", PhotonTargets.AllViaServer, holdPosValue);
        //}
        if (col2.gameObject.tag == "Bullet" && playerControlled == true)
        {
            GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer);
     
            scoreKeeper = GameObject.Find("scorekeeper(Clone)");
            scoreKeeper.GetComponent<ScoreKeeper>().ScoreAHit(col2.gameObject.GetComponent<Bullet>().owner);
            //scoreKeeper.GetComponent<PhotonView>().RPC("Score", PhotonTargets.AllBufferedViaServer, col2.gameObject.GetComponent<Bullet>().owner);
            //Destroy(col2.gameObject);
           
        }
    }
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
            playerControlled = true;
            pilot = whoUsedMe;
            
            whoUsedMe.active = false;
            myCam.active = true;
            GetComponent<PhotonView>().RPC("TakeOff", PhotonTargets.AllBufferedViaServer, whoUsedMe.GetComponent<PhotonView>().owner.ID);
        }
    }
}
