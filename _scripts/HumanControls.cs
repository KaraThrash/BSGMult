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
    public GameObject wrenchModel;
    public GameObject camWrenchModel;
    public float h;
    public float v;
    public float camGunAimAngle;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_TransformView = GetComponent<PhotonTransformView>();
       
    }
    public void SetAsMyPlayer() {
       
        cam.GetComponent<Camera>().enabled = true;
        cam.GetComponent<FPScamera>().enabled = true;
        gunModel.active = false;
        camGunModel.active = true;
        camWrenchModel.active = false;
        wrenchModel.active = false;
        charModel.active = false;
        controlled = true;
        GetComponent<PlayerCharacter>().controlled = true;
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if (jumpClock > 0) { jumpClock -= Time.deltaTime; }
        else {
            CheckGround();
            if (grounded == false)
            { transform.position = Vector3.MoveTowards(transform.position, downObject.transform.position, 3 * (airTime + 0.1f) * Time.deltaTime); }
        }


        if (controlled == true)
        {
            if (canMove == true)
            {
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { Move(); }

                if (Input.GetKey(KeyCode.Space)) { Jump(); }
                if (coolDown <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        GetComponent<PhotonView>().RPC("ChangeItem", PhotonTargets.AllViaServer);
                    }
                }
                else { coolDown -= Time.deltaTime; }
                if (cam.transform.localEulerAngles.x >= 270)
                {
                    camGunAimAngle = cam.transform.localEulerAngles.x - 360;

                }
                else
                {
                    camGunAimAngle = cam.transform.localEulerAngles.x;

                }
                GetComponent<PhotonView>().RPC("UpdateAimAnimationValues", PhotonTargets.AllViaServer, camGunAimAngle);
                if (Input.GetMouseButtonDown(0))
                {
                    if (anim.GetBool("RifleOut") == true) { GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer); }
                    else { GetComponent<PhotonView>().RPC("SwingWrench", PhotonTargets.AllViaServer); }


                }



            }
            if (Input.GetKey(KeyCode.Backspace))
            {
                canMove = true;
            }

           



           
        }
       // anim.SetFloat("CamAngle", camGunAimAngle);
    }
    [PunRPC]
    public void ChangeItem( )
    {
        GetComponent<Animator>().ResetTrigger("SwingWrench") ;
       
        if (controlled == true)
        {
            if (camWrenchModel.active == true)
            {
                anim.SetBool("RifleOut", true);
                camWrenchModel.active = false;
                camGunModel.active = true;
            }
            else
            {
                anim.SetBool("RifleOut", false);
                camWrenchModel.active = true;
                camGunModel.active = false;
            }
        }
        else
        {
            camWrenchModel.active = false;
            camGunModel.active = false;
            if (wrenchModel.active == true)
            {
                GetComponent<Animator>().SetBool("RifleOut", true);
                wrenchModel.active = false; gunModel.active = true;

            }
            else
            {
                GetComponent<Animator>().SetBool("RifleOut", false);
                gunModel.active = false; wrenchModel.active = true;

            }
        }
    }
    [PunRPC]
    public void UpdateAimAnimationValues(float aimAngle)
    {
        GetComponent<Animator>().SetFloat("CamAngle", aimAngle);
    }
    [PunRPC]
    public void SwingWrench()
    {
        GetComponent<Animator>().SetTrigger("SwingWrench");
    }
    [PunRPC]
    public void UpdateAnimationValues(float newH, float newV)
    {

        fwdObject.transform.localPosition = new Vector3(newH, 0, newV);
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
            v = Input.GetAxis("Vertical");
            h = Input.GetAxis("Horizontal");

            fwdObject.transform.localPosition = new Vector3(h, 0, v);


            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
            GetComponent<PhotonView>().RPC("UpdateAnimationValues", PhotonTargets.AllViaServer,h,v);



        }
    }
    public void CheckGround()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
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
                //if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true)
                //{ GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllViaServer, col.GetComponent<LocationChange>().myParent.name); }

                //else
                //{
                //    //transform.parent = null;
                //    GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllViaServer);
                //}
               // transform.position = col.gameObject.GetComponent<LocationChange>().exit.transform.position;
            }
        }

    }
 
    void Jump()
    {
        if (grounded == true && jumpClock <= 0)
        { jumpClock = 1; rb.AddForce(transform.up * 100 * Time.deltaTime, ForceMode.Impulse); }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        else
        {
            transform.rotation = (Quaternion)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }


}
