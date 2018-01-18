using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViperControls : Photon.PunBehaviour
{
   
    public GameObject pilot;
    public int playerNumber;
    public bool flying;
    PhotonView m_PhotonView;
    private Rigidbody rb;
    public float lift;
    public bool exit;
    public float hort;
    public float vert;
    public float roll;
    public float mouseX;
    public float mouseY;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float gunCoolDown;
    public bool xwing;
    public bool playerControlled;
    public float afterBurner;
    private Vector3 holdPosValue;
    public int turnSpeed;
    public int rollSpeed;
    public int flySpeed;
    public int liftSpeed;
    public int strafeSpeed;
    public bool controlled;
    public GameObject landingGear;
    public List<GameObject> bulletsByOwner = new List<GameObject>();
    public GameObject gunAnimation;

    public float myDrag;
    public float myAngularDrag;
    public GameObject afterBurnerIndicator;
    public GameObject glideIndicator;
    public float range;
    // Use this for initialization
    void Start () {
        range = 1000.0f;
        rb = GetComponent<Rigidbody>();
        m_PhotonView = GetComponent<PhotonView>();
   
        
    }

    // Update is called once per frame
    void Update()
    {
        flying = GetComponent<Fighter>().flying;
        if (flying == true)
        {
            //Secondary check to make sure the ship isnt active when docked
            if (transform.position.y < -500 && transform.position.y > -800) {

                GetComponent<Fighter>().OutOfBounds();

            }
            if (m_PhotonView.isMine == true )
            {

                if (xwing == true) { MouseFlightControls(); } else { KeyboardFlightControls(); }

                //if (Input.GetKeyDown(KeyCode.T))
                //{ GetComponent<PhotonView>().RPC("DeployLandingGear", PhotonTargets.AllViaServer, true); }

            }
            else { hort = 0; vert = 0; }

        }
        else {  }
        if (gunCoolDown > 0) { gunCoolDown -= Time.deltaTime; }

        
    }

   public void  MouseFlightControls()
    {
        if (Input.GetMouseButton(0))
        {
            if (gunCoolDown <= 0 && rb.velocity.magnitude < 250)
            {
                // gunAnimation.active = false;
                //  gunAnimation.active = true;
                RaycastShootGuns();
                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer, GetComponent<Rigidbody>().velocity);
                gunCoolDown = 0.1f;
            }
            gunCoolDown = gunCoolDown - Time.deltaTime;
        }

            if (Input.GetKeyUp(KeyCode.T) && flying == true)
        {
            GetComponent<Fighter>().Land();
          
        }
        if (Input.GetKey(KeyCode.Space)) { lift = liftSpeed; } else if (Input.GetKey(KeyCode.LeftShift)) { lift = -liftSpeed; } else { lift = 0; }
        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Q)) { roll = -rollSpeed; } else if (Input.GetKey(KeyCode.E)) { roll = rollSpeed; } else { roll = 0; }
        if (Input.GetMouseButton(1))
        { mouseX = 0; mouseY = 0; }
        else
        {
            //To prevent issues with going off window
            if (Input.mousePosition.x < 600 && Input.mousePosition.x > 20) { mouseX = Input.mousePosition.x - 600; } else if (Input.mousePosition.x > 700 && Input.mousePosition.x < 1200) { mouseX = Input.mousePosition.x - 700; } else { mouseX = 0; }
            if (Input.mousePosition.y < 325 && Input.mousePosition.y > 25) { mouseY = Input.mousePosition.y - 325; } else if (Input.mousePosition.y > 475 && Input.mousePosition.y < 855) { mouseY = Input.mousePosition.y - 475; } else { mouseY = 0; }
            // GetComponent<PhotonView>().RPC("flightControls", PhotonTargets.AllViaServer, vert, hort, roll, (mouseX * 0.5f), (-mouseY * 0.5f), exit, lift);
            Mathf.Clamp(mouseX, -50.0F, 50.0F);
            Mathf.Clamp(mouseY, -50.0F, 50.0F);
        }
        flightControls(vert, hort, roll, mouseX, -mouseY, exit, lift);
    }
    
    public void KeyboardFlightControls()
    {
        if (Input.GetKey(KeyCode.Space)) {
            
            if (gunCoolDown <= 0)
            {
                // gunAnimation.active = true;
                RaycastShootGuns();
                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer, GetComponent<Rigidbody>().velocity);
                gunCoolDown = 0.1f;
            }
            gunCoolDown -= Time.deltaTime;
        }
    
            if (Input.GetKeyUp(KeyCode.T) && flying == true)
         { GetComponent<Fighter>().Land(); }
        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.LeftBracket)) { lift = liftSpeed; } else if (Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.RightBracket)) { lift = -liftSpeed; } else { lift = 0; }

        hort = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        // if (Input.GetKey(KeyCode.KeypadPlus)) { vert += Time.deltaTime; }
        //if (Input.GetKey(KeyCode.KeypadMinus)) { vert -= Time.deltaTime; }
        if (Input.GetKey(KeyCode.Keypad7) || Input.GetKey(KeyCode.U)) { roll = -rollSpeed; } else if (Input.GetKey(KeyCode.Keypad9) || Input.GetKey(KeyCode.O)) { roll = rollSpeed; } else { roll = 0; }
        if (Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.J)) { mouseX = -turnSpeed; } else if (Input.GetKey(KeyCode.Keypad6) || Input.GetKey(KeyCode.L)) { mouseX = turnSpeed; } else { mouseX = 0; }
        if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.K)) { mouseY = -turnSpeed; } else if (Input.GetKey(KeyCode.Keypad8) || Input.GetKey(KeyCode.I)) { mouseY = turnSpeed; } else { mouseY = 0; }
        if (Input.GetKey(KeyCode.Keypad5) || Input.GetKey(KeyCode.P)) { afterBurner = 8.0f;afterBurnerIndicator.active = true; }
        if (afterBurner > 1) { afterBurner -= 0.4f; } else { afterBurner = 1; }

        if (Input.GetKeyDown(KeyCode.LeftShift)){
            ToggleGlide();
        }
       
        // GetComponent<PhotonView>().RPC("flightControls", PhotonTargets.AllViaServer, vert, hort, roll, mouseX, mouseY, exit, lift);
        flightControls(vert, hort, roll, mouseX, mouseY, exit, lift);
    }

    public void RaycastShootGuns()
    {

        RaycastHit hit;


        if (Physics.Raycast(gun1.transform.position, -gun1.transform.forward, out hit, range))
        {

            if (hit.transform.gameObject.tag == "Raider")
            // if (hit.transform.gameObject.GetComponent<Fighter>() != null || hit.transform.gameObject.GetComponent<Dradis>() != null)
            {

                hit.transform.gameObject.GetComponent<Raider>().TakeDamage(1, 1);
                // hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1,GetComponent<Fighter>().playerNumber);
            }
            else if (hit.transform.gameObject.tag == "Fighter")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
               // hit.transform.gameObject.GetComponent<Fighter>().TakeDamage(1, playerNumber);
            }
            else if (hit.transform.gameObject.tag == "TargetableSystem") {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
              //  hit.transform.gameObject.GetComponent<PartOfShip>().TakeDamage(1, playerNumber);
            }
        }

        if (Physics.Raycast(gun2.transform.position, -gun2.transform.forward, out hit, range))
        {

            if (hit.transform.gameObject.tag == "Raider")
            // if (hit.transform.gameObject.GetComponent<Fighter>() != null || hit.transform.gameObject.GetComponent<Dradis>() != null)
            {

                hit.transform.gameObject.GetComponent<Raider>().TakeDamage(1, 1);
                // hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1,GetComponent<Fighter>().playerNumber);
            }
            else if (hit.transform.gameObject.tag == "Fighter")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
                // hit.transform.gameObject.GetComponent<Fighter>().TakeDamage(1, playerNumber);
            }
            else if (hit.transform.gameObject.tag == "TargetableSystem")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
                //  hit.transform.gameObject.GetComponent<PartOfShip>().TakeDamage(1, playerNumber);
            }
        }
    }
    [PunRPC]
    public void ShootGuns(Vector3 currentVelocity)
    {
        Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
        Instantiate(bullet, gun1.transform.position, gun1.transform.rotation);


                //GameObject clone = Instantiate(bullet, gun2.transform.position, gun2.transform.rotation) as GameObject;
       // clone.GetComponent<Rigidbody>().velocity += currentVelocity;
       // clone.GetComponent<Bullet>().owner = GetComponent<PhotonView>().ownerId;
       // GameObject clone2 =  Instantiate(bullet, gun1.transform.position, gun1.transform.rotation) as GameObject;
      //  clone2.GetComponent<Rigidbody>().velocity += currentVelocity;
       // clone2.GetComponent<Bullet>().owner = GetComponent<PhotonView>().ownerId;
    }
    [PunRPC]
    public void DeployLandingGear(bool GearOutOrIn)
    {
        if (GearOutOrIn == true) { landingGear.active = true; }
        else { landingGear.active = false; } 


    }
    public void ToggleGlide()
    {

        if (rb.drag == 0) { rb.drag = myDrag; rb.angularDrag = myAngularDrag; glideIndicator.active = false; } else { vert = 0; rb.drag = 0; rb.angularDrag = myAngularDrag; glideIndicator.active = true; }
    }

    //[PunRPC]
    public void flightControls(float newvert,float newhort,float roll,float rollX,float rollY,bool leave, float lift) {
        if (vert != 0)
        {
            rb.AddForce(transform.forward * ((-vert * flySpeed * afterBurner)) * Time.deltaTime) ;
            if (rb.drag == 0) { rb.drag = myDrag; rb.angularDrag = myAngularDrag; glideIndicator.active = false; }
        }
        if (hort != 0)
        {
            
            rb.AddForce(transform.right * (-hort * strafeSpeed) * Time.deltaTime, ForceMode.Impulse);
        }
        //TODO: two monitors makes the X value freak out
        //Mathf.Clamp(rollX, -50.0F, 50.0F);
        //Mathf.Clamp(rollY, -50.0F, 50.0F);
        if (roll != 0) { rb.AddTorque(transform.forward * roll * Time.deltaTime, ForceMode.Impulse); }
        if (rollX != 0) { rb.AddTorque(transform.up * rollX * Time.deltaTime, ForceMode.Impulse); }
        if (rollY != 0) { rb.AddTorque(transform.right * -rollY * Time.deltaTime, ForceMode.Impulse); }
       // transform.Rotate(0.1f * -rollY, 0.1f * rollX, 0.1f * roll);

        if (lift != 0) { rb.AddForce(transform.up * lift * 30 * Time.deltaTime); }
        // if (leave == true &&  GetComponent<Rigidbody>().velocity.magnitude < 30) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.All); }
       GetComponent<PhotonView>().RPC("SyncVelocity", PhotonTargets.Others, rb.velocity);
    }
    [PunRPC]
    public void SyncVelocity(Vector3 newVelocity)
    { GetComponent<Rigidbody>().velocity = newVelocity; }
    //TODO: way back into the ship without jumping? someone needs to retract the pods?


    public void OnCollisionEnter(Collision col2)
    {

    }




    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           // stream.SendNext(flying);
          //  stream.SendNext(transform.position);
        }
        else
        {
            // flying = (bool)stream.ReceiveNext();
           
              //  transform.position = (Vector3)stream.ReceiveNext();
            
        }
    }

}
