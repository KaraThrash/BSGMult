﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylonFighterControls : Photon.PunBehaviour
{

    public GameObject pilot;
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

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        m_PhotonView = GetComponent<PhotonView>();


    }

    // Update is called once per frame
    void Update()
    {
        flying = GetComponent<Fighter>().flying;
        if (flying == true)
        {

        }
        else { }
        if (gunCoolDown > 0) { gunCoolDown -= Time.deltaTime; }

        if (m_PhotonView.isMine == true && flying == true)
        {
            if (xwing == true) { MouseFlightControls(); } else { KeyboardFlightControls(); }
        }
        else { hort = 0; vert = 0; }
    }

    public void MouseFlightControls()
    {
        if (Input.GetMouseButton(0))
        {
            if (gunCoolDown <= 0 && rb.velocity.magnitude < 150)
            {
                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                gunCoolDown = 0.2f;
            }
            gunCoolDown -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.T) && GetComponent<Fighter>().currentHangar != null) {
            if (GetComponent<Fighter>().currentHangar != null)
            {
                GetComponent<PhotonView>().RPC("Land", PhotonTargets.AllBufferedViaServer, GetComponent<Fighter>().currentHangar.transform.name);
            }
            else { GetComponent<PhotonView>().RPC("Land", PhotonTargets.AllBufferedViaServer, "none"); }
        }
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
        if (Input.GetKey(KeyCode.Space))
        {
            if (gunCoolDown <= 0 && rb.velocity.magnitude < 150)
            {
                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                gunCoolDown = 0.2f;
            }
            gunCoolDown -= Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.T) && GetComponent<Fighter>().currentHangar != null)
        {
            if (GetComponent<Fighter>().currentHangar != null)
            {
                GetComponent<PhotonView>().RPC("Land", PhotonTargets.AllBufferedViaServer, GetComponent<Fighter>().currentHangar.transform.name);
            }
            else { GetComponent<PhotonView>().RPC("Land", PhotonTargets.AllBufferedViaServer, "none"); }
        }
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



    // [PunRPC]
    public void flightControls(float vert, float hort, float roll, float rollX, float rollY, bool leave, float lift)
    {
        if (vert != 0)
        {
            rb.AddForce(transform.forward * ((-vert * 300) - afterBurner));

        }
        if (hort != 0)
        {
            rb.AddForce(transform.right * (-hort * 5), ForceMode.Impulse);
        }
        //TODO: two monitors makes the X value freak out
        Mathf.Clamp(rollX, -50.0F, 50.0F);
        Mathf.Clamp(rollY, -50.0F, 50.0F);
        if (roll != 0) { rb.AddTorque(transform.forward * roll * Time.deltaTime, ForceMode.Impulse); }
        if (rollX != 0) { rb.AddTorque(transform.up * rollX * Time.deltaTime, ForceMode.Impulse); }
        if (rollY != 0) { rb.AddTorque(transform.right * -rollY * Time.deltaTime, ForceMode.Impulse); }
        if (lift != 0) { rb.AddForce(transform.up * lift * 30); }
        // if (leave == true &&  GetComponent<Rigidbody>().velocity.magnitude < 30) { GetComponent<PhotonView>().RPC("Land", PhotonTargets.All); }

    }

    //TODO: way back into the ship without jumping? someone needs to retract the pods?


    public void OnCollisionEnter(Collision col2)
    {

    }




    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           // stream.SendNext(flying);
            stream.SendNext(transform.position);
        }
        else
        {
            //flying = (bool)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }

}