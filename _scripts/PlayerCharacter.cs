using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCharacter : Photon.PunBehaviour
{
    //using this for humanoid
    public int hp;
    public GameObject hpHud;
    public GameObject testObj;
    PhotonView m_PhotonView;
    public GameObject myCamera;
    public GameObject gun;
    public GameObject bullet;
    public GameObject station;
    public bool flying;
    public bool atComputer;
    public bool controlled;
    public GameObject galactica;
    public GameObject jumpManager;
    public GameObject localPlayer;
    public GameObject spaceObject;
    public GameObject masterShipList;
    public GameObject backpack;
    public GameObject carriedObject;
    // Use this for initialization
    void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();

       }
 

    // Update is called once per frame
    void Update()
    {

        if (controlled == true)
        {
           
            if (Input.GetKey(KeyCode.Backspace))
            {
                if (station != null)
                {
                    myCamera.active = true;
                    station.GetComponent<PhotonView>().RPC("MakeAvailable", PhotonTargets.AllBufferedViaServer);
                }

            }
            if (Input.GetMouseButtonDown(0) )
            {

               // GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);

            }
            if (Input.GetKey(KeyCode.T))
            {

                GetComponent<PhotonView>().RPC("DropCarriedObject", PhotonTargets.AllViaServer);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                CheckForIneractableObject();
            }
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
        if (localPlayer != null) { localPlayer.GetComponent<PlayerMain>().shipGroup = 3; }
        flying = true;
        this.gameObject.active = false;
        
    }
    [PunRPC]
    public void GetOutShip(int shipOnList)
    {
        flying = false;
        transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(shipOnList).transform;
        this.gameObject.active = true;
        if (localPlayer != null) { myCamera.active = true; localPlayer.GetComponent<PlayerMain>().shipGroup = shipOnList; }
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
                    GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllBufferedViaServer, col.GetComponent<LocationChange>().shipOnList);
                }

                else
                {
                    //transform.parent = null;
                    //TODO: right now the only way a player leaves a ship is in a fighter so if they are on the hangar or the bridge they should still jump
                    GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllBufferedViaServer);
                }
                transform.position = col.gameObject.GetComponent<LocationChange>().exit.transform.position;
                transform.rotation = col.gameObject.GetComponent<LocationChange>().exit.transform.rotation;
                
            }
        }

    }
    public void OnCollisionEnter(Collision col2)
    {
        if (col2.gameObject.tag == "Bullet")
        {
            hp--;
            if (hp > 0 && photonView.isMine == true)
            {
                hpHud.transform.GetChild(hp).gameObject.active = false;
            }
            if (hp <= 0) {
                transform.localPosition = Vector3.zero; hp = 3;
                hpHud.transform.GetChild(0).gameObject.active = true;
                hpHud.transform.GetChild(1).gameObject.active = true;
                hpHud.transform.GetChild(2).gameObject.active = true;
            }
        }
    }



    [PunRPC]
    public void ParentToShip(int newShipParent) { transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(newShipParent).transform; }
    [PunRPC]
    public void NoParent() { transform.parent = null; }

    public void TakeOff() { myCamera.active = false; GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer); }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

            stream.SendNext(flying);
        }
        else
        {

            flying = (bool)stream.ReceiveNext();
        }
    }
    public void JumpEffects(int coords,int newShipGroup) {
       // myCamera.GetComponent<CameraEffects>().StartFTLEffect();
        localPlayer.GetComponent<PlayerMain>().spaceCoordinates = coords;
        localPlayer.GetComponent<PlayerMain>().Jumping(coords, newShipGroup);
    }
    public void JumpCameraEffects()
    {
        myCamera.GetComponent<CameraEffects>().StartFTLEffect();
       // localPlayer.GetComponent<PlayerMain>().spaceCoordinates = coords;
        //localPlayer.GetComponent<PlayerMain>().Jumping(coords);
    }

    [PunRPC]
    public void PickedUpObject() {
        backpack.active = true;
        RaycastHit hit;
        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 5.0f) && hit.transform.gameObject.GetComponent<SupplyCrate>() != null)
        {
            hit.transform.gameObject.SendMessage("Interact", this.gameObject);

        }
        
    }
    [PunRPC]
    public void DropCarriedObject()
    {
        backpack.active = false;
        if (carriedObject != null)
        {
            if (transform.parent != null) { carriedObject.transform.parent = transform.parent; } else { carriedObject.transform.parent = null; }
            
            carriedObject.transform.position = gun.transform.position;
            carriedObject.active = true;
            carriedObject.GetComponent<Rigidbody>().AddForce(gun.transform.forward * 10,ForceMode.Impulse );
            
        }
        carriedObject = null;
        
        //transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(newShipParent).transform;
    }
}
