using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanControls : Photon.PunBehaviour
{

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

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_TransformView = GetComponent<PhotonTransformView>();
        //cam = GameObject.Find("RPG Camera");
    }
    public void SetAsMyPlayer() {
       // cam.active = true;
        cam.GetComponent<Camera>().enabled = true;
        cam.GetComponent<FPScamera>().enabled = true;
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
            if (grounded == false) { transform.position = Vector3.MoveTowards(transform.position, downObject.transform.position, 2 * Time.deltaTime); }
            
        }
        ApplySynchronizedValues();
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
                float h = Input.GetAxis("Horizontal");
                transform.position += transform.right * Time.deltaTime * speed * h;
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                float v = Input.GetAxis("Vertical");
                transform.position += transform.forward * Time.deltaTime * speed * v;
            }
  
            
            

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
    [PunRPC]
    public void ParentToShip(string newShipParent) { transform.parent = GameObject.Find(newShipParent).transform; }
    [PunRPC]
    public void NoParent() { transform.parent = null; }
    void Jump()
    {
        if (grounded == true)
        { rb.AddForce(transform.up * 100 * Time.deltaTime, ForceMode.Impulse); }
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
