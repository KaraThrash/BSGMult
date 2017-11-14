﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeShip : Photon.PunBehaviour
{
    public Transform peopleOnBoard;
    public bool fleetShip;
    public bool manned;
    public float speed;

    public int hp;
    public GameObject myCamera;
    public GameObject fwdObject;
    public GameObject rotationObject;
    public GameObject engineAnim;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, 1.0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationObject.transform.rotation, 2.0f * Time.deltaTime);
        fwdObject.transform.position = Vector3.MoveTowards(fwdObject.transform.position, transform.position, 2.0f);
        if (Vector3.Distance(fwdObject.transform.position, transform.position) > 10) { engineAnim.active = true; } else { engineAnim.active = false; }
        if (manned == true)
        {
            BeingFlown();
        }


    }
    public void BeingFlown()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<PhotonView>().RPC("SetForwardObject", PhotonTargets.AllViaServer);
            //transform.Translate(Vector3.right * 1);
        }

        if (Input.GetKey(KeyCode.S)) { rotationObject.transform.Rotate(0.5f, 0, 0); }
        if (Input.GetKey(KeyCode.W)) { rotationObject.transform.Rotate(-0.5f, 0, 0); }
        if (Input.GetKey(KeyCode.Q)) { rotationObject.transform.Rotate(0, 0, -0.5f); }
        if (Input.GetKey(KeyCode.E)) { rotationObject.transform.Rotate(0, 0, 0.5f); }
        if (Input.GetKey(KeyCode.D)) { rotationObject.transform.Rotate(0, 0.5f, 0); }
        if (Input.GetKey(KeyCode.A)) { rotationObject.transform.Rotate(0, -0.5f, 0); }
        GetComponent<PhotonView>().RPC("SetRotationObjects", PhotonTargets.Others, rotationObject.transform.rotation);
    }

    [PunRPC]
    public void SetRotationObjects(Quaternion rot) { rotationObject.transform.rotation = rot; }
    [PunRPC]
    public void SetForwardObject() { fwdObject.transform.localPosition = new Vector3(0, 0, -1000.0f); }

    public void Manned() { rotationObject.transform.rotation = transform.rotation; manned = true; myCamera.active = true; }
    public void NotManned() { manned = false; myCamera.active = false; rotationObject.transform.rotation = transform.rotation; }

    public void TakeDamage()
    {
        if (hp > 0)
        {
            hp--;

            if (hp <= 0 && fleetShip == true)
            {
                //Die();
                ForPassengersAfterDestroyed();
                this.GetComponent<FleetShip>().FleetShipDie();

                //GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllBufferedViaServer);
            }
            //Destroy(this.gameObject);
            Debug.Log("hit");
        }
    }
    public void ForPassengersAfterDestroyed()
    {
        
            foreach (Transform child in peopleOnBoard)
            {
                if (child.GetComponent<PlayerCharacter>().localPlayer != null && child.gameObject.active == true)
                {

                child.GetComponent<PlayerCharacter>().TakeDamage(99);
                }



            }
        
    }

}