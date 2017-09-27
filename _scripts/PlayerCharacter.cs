using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Photon.PunBehaviour
{
    //using this for humanoid
    PhotonView m_PhotonView;
    public GameObject myCamera;
    public GameObject gun;
    public GameObject bullet;
    public GameObject station;
    public bool flying;
    public bool atComputer;
    // Use this for initialization
    void Start()
    {
        transform.parent = GameObject.Find("Galactica(Clone)").transform;
        transform.position = GameObject.Find("Galactica(Clone)").GetComponent<Galactica>().shipInterior.transform.position;
        //myCamera = GameObject.Find("RPG Camera");
        m_PhotonView = GetComponent<PhotonView>();
      
    }
    // Use this for initialization


    // Update is called once per frame
    void Update()
    {
        if (m_PhotonView.isMine == true)
        {
            if (Input.GetKey(KeyCode.Backspace))
            {
                if (station != null)
                {
                    myCamera.active = true;
                    station.GetComponent<PhotonView>().RPC("MakeAvailable", PhotonTargets.AllViaServer);
                }

            }
            if (Input.GetMouseButtonDown(0) )
            {

                GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);

            }

        }
        if (Input.GetKey(KeyCode.E))
        {
            
            CheckForIneractableObject();
        }
    }
    public void CheckForIneractableObject()
    {
        RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f))
        //{
        //    Debug.Log(hit.transform.name);

        //}

        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 5.0f) && hit.transform.tag == "Interactable")
        {
            hit.transform.gameObject.SendMessage("Interact", this.gameObject);

        }

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
        if (photonView.isMine == true)
        {
            if (col.gameObject.tag == "Entrance")
            {
                //if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true) { transform.parent = col.GetComponent<LocationChange>().myParent.transform; }
                if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true)
                {
                    GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllViaServer, col.GetComponent<LocationChange>().myParent.name);
                }

                else
                {
                    //transform.parent = null;
                    //TODO: right now the only way a player leaves a ship is in a fighter so if they are on the hangar or the bridge they should still jump
                    GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllViaServer);
                }
                transform.position = col.gameObject.GetComponent<LocationChange>().exit.transform.position;
                transform.rotation = col.gameObject.GetComponent<LocationChange>().exit.transform.rotation;
                
            }
        }

    }
    [PunRPC]
    public void ParentToShip(string newShipParent) { transform.parent = GameObject.Find(newShipParent).transform; }
    [PunRPC]
    public void NoParent() { transform.parent = null; }

    public void TakeOff() { myCamera.active = false; GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer); }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(transform.position);
        }
        else
        {

            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
    public void JumpEffects() {
        myCamera.GetComponent<CameraEffects>().StartFTLEffect();
    }
}
