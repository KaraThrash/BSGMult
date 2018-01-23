using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanControls : Photon.PunBehaviour
{
    //This is the current movementyController for humanoid characters
    public GameObject heldItem;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
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
    public Vector3 moveDirection;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
        if (canMove == true)
        {
            
        }

        if (controlled == true)
        {
            CheckGround();
            if (jumpClock > 0) { jumpClock -= 0.20f; }
            else
            {
                jumpClock = 0;

            }
            Move();
            if (canMove == true)
            {
                
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {  }

                
                if (coolDown <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                       // GetComponent<PhotonView>().RPC("ChangeItem", PhotonTargets.AllViaServer);
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
                if (!Input.GetKey(KeyCode.Tab) && heldItem.GetComponent<HeldItem>().coolDown <= 0)
                {
                    if (Input.GetMouseButton(0))
                    {
                        GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                        if (anim.GetBool("RifleOut") == true && !Input.GetKey(KeyCode.Tab))
                        {
                            //GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                        }
                        else { GetComponent<PhotonView>().RPC("SwingWrench", PhotonTargets.AllViaServer); }


                    }
                    if (Input.GetMouseButton(1))
                    {
                        GetComponent<PhotonView>().RPC("UseHeldSecondary", PhotonTargets.AllViaServer);
                        if (anim.GetBool("RifleOut") == true && !Input.GetKey(KeyCode.Tab))
                        {
                            //GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                        }
                        else { GetComponent<PhotonView>().RPC("SwingWrench", PhotonTargets.AllViaServer); }


                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                canMove = true;

            }

            if (moveDirection != Vector3.zero) {
               // GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
            }
           



        }
       // anim.SetFloat("CamAngle", camGunAimAngle);
    }
    [PunRPC]
    public void ChangeItem(int changeTo)
    {
        GetComponent<Animator>().ResetTrigger("SwingWrench") ;
       
        if (controlled == true)
        {
            if (changeTo == 0)
            {
                heldItem.GetComponent<HeldItem>().ChangeType(1);
                anim.SetBool("RifleOut", true);
                camWrenchModel.active = false;
                camGunModel.active = true;
            }
            else
            {
                heldItem.GetComponent<HeldItem>().ChangeType(2);
                anim.SetBool("RifleOut", false);
                camWrenchModel.active = true;
                camGunModel.active = false;
            }
        }
        else
        {
            camWrenchModel.active = false;
            camGunModel.active = false;
            if (changeTo == 0)
            {
                heldItem.GetComponent<HeldItem>().ChangeType(1);
                GetComponent<Animator>().SetBool("RifleOut", true);
                wrenchModel.active = false; gunModel.active = true;

            }
            else
            {
                heldItem.GetComponent<HeldItem>().ChangeType(2);
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
        GetComponent<Animator>().Play("SwingSword");
    }

    [PunRPC]
    public void UpdateFromFPSCamera(Quaternion newRot)
    {
        
        transform.localRotation = newRot;
    }
    [PunRPC]
    public void UpdateAnimationValues(Vector3 newMoveDir,float newH, float newV)
    {

        fwdObject.transform.localPosition = new Vector3(newH, 0, newV);
        if (anim != null)
        {
            anim.SetFloat("h", newH);
            anim.SetFloat("v", newV);
        }
        moveDirection = newMoveDir;
       
    }

    void ApplySynchronizedValues()
    {
        m_TransformView.SetSynchronizedValues(transform.position, 1);
    }

    public void Move()
    {


        if (grounded == true)
        {
            if (canMove == true)
            {
                v = Input.GetAxis("Vertical");
                h = Input.GetAxis("Horizontal");




                fwdObject.transform.localPosition = new Vector3(h, 0, v);


                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);

                moveDirection *= speed;
                if (Input.GetKey(KeyCode.Space)) { moveDirection.y = jumpSpeed; }//Jump(); 
                                                                                 //if (grounded == true)
                                                                                 //{
                                                                                 //    moveDirection *= speed;

                //}
                // else { moveDirection *= (speed * 0.5f); }
            }
            else { h = 0; v = 0; moveDirection.x = 0; moveDirection.z = 0; }

        }
                moveDirection.y -= gravity * Time.deltaTime;

        GetComponent<PhotonView>().RPC("UpdateAnimationValues", PhotonTargets.Others, moveDirection, h,v);



        
    }


    public void CheckGround()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        RaycastHit raycastResult;
        
        if (Physics.Raycast(transform.position, fwd, out raycastResult, groundCheckDistance ))
        {    
            grounded = true;
            airTime = 0;
            jumpClock = -Vector3.Distance(transform.position, raycastResult.point);
            if (Vector3.Distance(transform.position, raycastResult.point) < 0.5f) { grounded = true; } else { grounded = false; }
            //jumpClock = 0;
            //GetComponent<CharacterController>().Move(raycastResult.point * Time.deltaTime);
        }
        else
        {
            grounded = false;
            if (airTime < 20) { airTime += 0.1f; }
           // GetComponent<CharacterController>().Move((downObject.transform.position - transform.position) * Time.deltaTime);

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
        {
            //rb.AddForce(transform.up * 10 * Time.deltaTime,ForceMode.Impulse);
            jumpClock = 3;
            grounded = false;
        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
          //  stream.SendNext(transform.rotation);
           // stream.SendNext(transform.position);
        }
        else
        {
           // transform.rotation = (Quaternion)stream.ReceiveNext();
           // transform.position = (Vector3)stream.ReceiveNext();
        }
    }


}
