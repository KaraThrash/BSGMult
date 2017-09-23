using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Photon.PunBehaviour
{
    //Using Player character for humanoid

    PhotonView m_PhotonView;
    public GameObject myCamera;
    public GameObject gun;
    public GameObject bullet;
    // Use this for initialization
    void Start()
    {
        
        myCamera = GameObject.Find("RPG Camera");
        m_PhotonView = GetComponent<PhotonView>();
    }
    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && photonView.isMine == true)
        {

            GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);

        }
        if (Input.GetKey(KeyCode.V) && m_PhotonView.isMine == true)
        {
            CheckForIneractableObject();
        }
    }
    public void CheckForIneractableObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f) && hit.transform.tag == "Interactable")
        { hit.transform.gameObject.SendMessage("Interact", this.gameObject); }

    }

    [PunRPC]
    public void ShootGuns()
    {

        Instantiate(bullet, gun.transform.position, gun.transform.rotation);
    }
    [PunRPC]
    public void GetInShip()
    {
        this.gameObject.active = false;
    }
    [PunRPC]
    public void GetOutShip()
    {
        this.gameObject.active = true;
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Entrance") { transform.position = col.gameObject.GetComponent<LocationChange>().exit.transform.position; }
            
    }
    public void TakeOff() { myCamera.active = false; GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer); }
}
