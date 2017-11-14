using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCharacter : Photon.PunBehaviour
{
    //using this for humanoid
    public int hp;
    public int ammo;
    public Text hpHud;
    public Text ammoHud;
    public GameObject itemMasterList;
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
    public int carriedObject;
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
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (carriedObject != -1)
                {
                    GetComponent<PhotonView>().RPC("DropCarriedObject", PhotonTargets.AllViaServer, carriedObject);
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {

                CheckForIneractableObject();
            }
        }
        
    }
    public void SetAsMyPlayer(GameObject myNewPlayer)
    {
        localPlayer = myNewPlayer;
        controlled = true;
        ammoHud.text = ammo.ToString();
        myNewPlayer.GetComponent<PlayerMain>().roundManager = jumpManager.GetComponent<JumpManager>().roundManager;
        myNewPlayer.GetComponent<PlayerMain>().roundManager.GetComponent<RoundManager>().localPlayer = myNewPlayer;
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

        if (ammo > 0) { Instantiate(bullet, gun.transform.position, gun.transform.rotation); ammo--; }
        if (controlled == true) { ammoHud.text = ammo.ToString(); }
    }
    [PunRPC]
    public void GetInShip()
    {
        if (localPlayer != null) { localPlayer.GetComponent<PlayerMain>().shipGroup = 3; }
        flying = true;
        this.gameObject.active = false;
        
    }
    [PunRPC]
    public void GetOutShip(int shipOnList, Vector3 newExit)
    {
        flying = false;
        transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(shipOnList).transform;
        transform.position = newExit;
        this.gameObject.active = true;
        if (localPlayer != null) {
            transform.position = newExit; this.gameObject.active = true;  myCamera.active = true; localPlayer.GetComponent<PlayerMain>().shipGroup = shipOnList;
            localPlayer.GetComponent<PhotonView>().RPC("SetHumanActive",PhotonTargets.AllViaServer);
        }
    }



    public void OnTriggerEnter(Collider col)
    {
        //if (photonView.isMine == true)
        //{
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
      //  }

    }

 

    public void OnCollisionEnter(Collision col2)
    {
        if (col2.gameObject.tag == "Bullet")
        {
            TakeDamage(1);
        }
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp > 0 && localPlayer != null)
        {
            hpHud.text.Remove(-1);
        }
        if (hp <= 0)
        {
            if (localPlayer != null) { localPlayer.GetComponent<PlayerMain>().roundManager.GetComponent<RoundManager>().wasFrakked = true; }
            galactica.GetComponent<Galactica>().medbay.GetComponent<Medbay>().PlaceInBed(this.gameObject);


        }

    }


    [PunRPC]
    public void ParentToShip(int newShipParent) { transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(newShipParent).transform; }
    [PunRPC]
    public void NoParent() { transform.parent = null; }

    public void TakeOff() {
        //myCamera.active = false;
        GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer);
    }

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
        if (station != null)
        {
            myCamera.active = true;
            station.GetComponent<PhotonView>().RPC("MakeAvailable", PhotonTargets.AllBufferedViaServer);
        }
        myCamera.GetComponent<CameraEffects>().StartFTLEffect();
       // localPlayer.GetComponent<PlayerMain>().spaceCoordinates = coords;
        //localPlayer.GetComponent<PlayerMain>().Jumping(coords);
    }

    [PunRPC]
    public void PickedUpObject(int itemPickedup) {
        carriedObject = itemPickedup;
        backpack.active = true; 
    }
    [PunRPC]
    public void ChangeAmmo(int ammoChange)
    {
        ammo += ammoChange;
        if (controlled == true)
        {
            ammoHud.text = ammo.ToString();
        }
    }
    [PunRPC]
    public void DropCarriedObject(int itemNumber)
    {
        backpack.active = false;
        itemMasterList.GetComponent<ItemList>().DropCrate(itemNumber,gun);
        
        carriedObject = -1;
        
        //transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(newShipParent).transform;
    }



    public void Heal(int hpChange)
    {
        hp += hpChange;
        while (hpHud.text.Length < hp)
        { hpHud.text += "i"; }
    }

}
