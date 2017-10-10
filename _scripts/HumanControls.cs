using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanControls : Photon.PunBehaviour
{
    //This is the current movementyController for humanoid characters
    public GameObject gun;
    public int speed;
    public float coolDown;
    private Vector3 dir;
    private Rigidbody rb;
    public bool grounded;
    public float groundCheckDistance;
    public GameObject cam;
    public float airTime;
    public bool canMove;
    public GameObject fwdObject;
    public GameObject downObject;
    public bool controlled;
    public GameObject raycastObject;
    PhotonTransformView m_TransformView;
    public float jumpClock;
    public GameObject charModel;
    public GameObject camGunModel;
    private Animator anim;
    public GameObject gunModel;
    private float h;
    private float v;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_TransformView = GetComponent<PhotonTransformView>();
        //cam = GameObject.Find("RPG Camera");
    }
    public void SetAsMyPlayer() {
       // cam.active = true;
        cam.GetComponent<Camera>().enabled = true;
        cam.GetComponent<FPScamera>().enabled = true;
        gunModel.active = false;
        camGunModel.active = true;
        charModel.active = false;
        controlled = true;

    }
    // Update is called once per frame
    void Update()
    {
        if (controlled == true)
        {
           // if (transform.parent != null) { cam.transform.parent = transform.parent; }
            //print(rb.velocity.magnitude);
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { Move(); }

           // CheckGround();
            if (Input.GetKey(KeyCode.Space)) { Jump(); }
            if (coolDown <= 0)
            {
                if (Input.GetKey(KeyCode.Q))
                {

                }
            }
            else { coolDown -= Time.deltaTime; }

            if (Input.GetKey(KeyCode.Escape))
            {
                canMove = true;
            }
           // if (Input.GetKey(KeyCode.V) )
           // {
               // CheckForIneractableObject();
            //}
            CheckGround();
            if (grounded == false && jumpClock <= 0) { transform.position = Vector3.MoveTowards(transform.position, downObject.transform.position, 3 * Time.deltaTime); }
            if (jumpClock > 0) { jumpClock -= Time.deltaTime; }
        }
        
        if (cam.transform.eulerAngles.x >= 270)
        {
            Debug.Log(cam.transform.eulerAngles.x);
            
            anim.SetFloat("CamAngle", cam.transform.eulerAngles.x - 360 );
        }
        else
        {
            Debug.Log("less");
            anim.SetFloat("CamAngle", cam.transform.eulerAngles.x);
            // anim.SetFloat("CamAngleNeg", 0);
        }
       // anim.SetFloat("CamAngle", cam.transform.eulerAngles.x);
      //  anim.SetFloat("CamAngleNeg", cam.transform.eulerAngles.x - 360);

    }
    [PunRPC]
    public void UpdateAnimationValues(float newH, float newV)
    {

        anim.SetFloat("h", newH);
        anim.SetFloat("v", newV);
        
    }

    void ApplySynchronizedValues()
    {
        m_TransformView.SetSynchronizedValues(transform.position, 1);
    }

    public void Move()
    {

        if (canMove == true)
        {
            if (Input.GetAxis("Horizontal") != 0) {
                 h = Input.GetAxis("Horizontal");
                transform.position += transform.right * Time.deltaTime * speed * h;
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                 v = Input.GetAxis("Vertical");
                transform.position += transform.forward * Time.deltaTime * speed * v;
            }
            GetComponent<PhotonView>().RPC("UpdateAnimationValues", PhotonTargets.AllViaServer,h,v);



        }
    }
    public void CheckGround()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        //Physics.Raycast(transform.position, fwd, groundCheckRange) ||
        if (Physics.Raycast(transform.position, fwd, groundCheckDistance))
        {
            
            grounded = true;
            airTime = 0;
            
        }
        else
        {
            grounded = false;
            if (airTime < 20) { airTime += 0.1f; }
            

        }
    }
    public void OnCollisionEnter(Collision collision)
    {
       // grounded = true;
    }
    public void OnTriggerEnter(Collider col)
    {
        if (photonView.isMine == true)
        {
            if (col.gameObject.tag == "Entrance")
            {
                //if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true) { transform.parent = col.GetComponent<LocationChange>().myParent.transform; }
                if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true)
                { GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllViaServer, col.GetComponent<LocationChange>().myParent.name); }

                else
                {
                    //transform.parent = null;
                    GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllViaServer);
                }
                transform.position = col.gameObject.GetComponent<LocationChange>().exit.transform.position;
            }
        }

    }
 
    void Jump()
    {
        if (grounded == true)
        { jumpClock = 1; rb.AddForce(transform.up * 100 * Time.deltaTime, ForceMode.Impulse); }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           // stream.SendNext(transform.rotation);
           // stream.SendNext(transform.position);
        }
        else
        {
           // transform.rotation = (Quaternion)stream.ReceiveNext();
           // transform.position = (Vector3)stream.ReceiveNext();
        }
    }

    public void CheckForIneractableObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(raycastObject.transform.position, raycastObject.transform.forward, out hit, 5.0f) && hit.transform.tag == "Interactable")
        {
            hit.transform.gameObject.SendMessage("Interact", this.gameObject);

        }

    }

}
