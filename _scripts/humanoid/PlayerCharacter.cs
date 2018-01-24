using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCharacter : Photon.PunBehaviour
{
    //using this for humanoid
    public int myCharacterOnList; //who they are in the character masterList
    public int playerNumber;
    public int shipGroup;
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
    public bool controlled;
    
    public GameObject galactica;
    public GameObject jumpManager;
    public GameObject localPlayer;
    public GameObject masterShipList;
    public GameObject masterCharacterList;
    public GameObject backpack;
    public int carriedObject;
    public int heldItem; //0gun, 1 wrench
    // Use this for initialization
    void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();
        ammo = 10;
    }
 

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1500) {

            GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 99,0);
        }
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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (heldItem == 0)
                { heldItem = 1; }
                else { heldItem = 0; }
                GetComponent<PhotonView>().RPC("ChangeItem", PhotonTargets.AllViaServer,heldItem);
            }
            if (Input.GetMouseButtonDown(0))
            {

                UseHeldItem(0);
               // GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);

            }
            if (Input.GetMouseButtonDown(1))
            {

                UseHeldItem(1);
                // GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);

            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (carriedObject != -1)
                {
                    
                    GetComponent<PhotonView>().RPC("DropCarriedObject", PhotonTargets.AllViaServer, carriedObject);
                    carriedObject = -1;
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                CheckForIneractableObject();
            }
        }
        
    }
    public void SetAsMyPlayer(GameObject myNewPlayer,int newPlayerNumber)
    {
        playerNumber = newPlayerNumber;
        localPlayer = myNewPlayer;
        controlled = true;
        ammoHud.text = GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ammo.ToString();
        myNewPlayer.GetComponent<PlayerMain>().roundManager = jumpManager.GetComponent<JumpManager>().roundManager;
        myNewPlayer.GetComponent<PlayerMain>().roundManager.GetComponent<RoundManager>().localPlayer = myNewPlayer;
    }
    public void UseHeldItem(int leftOrRightClick)
    {
        RaycastHit hit;

        if (heldItem == 0 && GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ShootGun() == true)
        {
            
             ammoHud.text = GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ammo.ToString();
            if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 30.0f) )
            {
                //hit.transform.gameObject.SendMessage("Interact", this.gameObject);
                
                if (hit.transform.tag == "Player")
                { hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, playerNumber); }
                else if (hit.transform.tag == "TargetableSystem")
                {
                    hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, playerNumber);
                    //hit.transform.gameObject.GetComponent<PartOfShip>().TakeDamage(1, GetComponent<Fighter>().playerNumber);
                }
            }
            
        }
        else {
            if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 3.0f))
            {
                //hit.transform.gameObject.SendMessage("Interact", this.gameObject);

                if (hit.transform.tag == "Player" || hit.transform.tag == "TargetableSystem")
                { hit.transform.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 3, playerNumber); }
                 else if (hit.transform.tag == "Interactable") {
                    if (leftOrRightClick == 0)
                    { hit.transform.GetComponent<PhotonView>().RPC("Repair", PhotonTargets.AllViaServer, playerNumber); }
                    else { hit.transform.GetComponent<PhotonView>().RPC("Sabotage", PhotonTargets.AllViaServer, playerNumber); }
                   
                }
            }
        }

    }
    public void CheckForIneractableObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(myCamera.transform.position, myCamera.transform.forward, out hit, 5.0f) && hit.transform.tag == "Interactable")
        {
            hit.transform.gameObject.SendMessage("Interact", this.gameObject);

        }

    }
    

    [PunRPC]
    public void ShootGuns()
    {
        GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().UsePrimary();
        //if (ammo > 0) { Instantiate(bullet, gun.transform.position, gun.transform.rotation); ammo--; }
        if (controlled == true) {  }
    }
    [PunRPC]
    public void UseHeldSecondary()
    {
        GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().UseSecondary();
        //if (ammo > 0) { Instantiate(bullet, gun.transform.position, gun.transform.rotation); ammo--; }
        if (controlled == true) { ammoHud.text = GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ammo.ToString(); }
    }

   // [PunRPC]
    public void GetInShip()
    {
        if (localPlayer != null) { localPlayer.GetComponent<PlayerMain>().shipGroup = 3; }
        flying = true;
        
        this.gameObject.active = false;
        
    }
    [PunRPC]
    public void GetOutShip(int shipOnList, Vector3 newExit,int newShipGroup)
    {

        masterCharacterList.GetComponent<PhotonView>().RPC("EnableCharacter", PhotonTargets.AllViaServer,myCharacterOnList);
        flying = false;
        transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(shipOnList).transform;
        GetComponent<HumanControls>().grounded = false;
        transform.position = newExit;
        shipGroup = newShipGroup;
        this.gameObject.active = true;
        if (localPlayer != null) {
            hpHud.text = "";
            Heal(0);
            ammoHud.text  = GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ammo.ToString();
            transform.position = newExit;
            this.gameObject.active = true;
            myCamera.active = true;
            localPlayer.GetComponent<PlayerMain>().shipGroup = newShipGroup;

            localPlayer.GetComponent<PhotonView>().RPC("SetHumanActive",PhotonTargets.AllViaServer);
        }
    }



    public void OnTriggerEnter(Collider col)
    {
        //if (photonView.isMine == true)
        //{
            if (col.gameObject.tag == "Entrance")
            {
            
                if (col.GetComponent<LocationChange>().parentObjectThatUsesMe == true)
                {
                   //debug commented out to use teleporters : GetComponent<PhotonView>().RPC("ParentToShip", PhotonTargets.AllBufferedViaServer, col.GetComponent<LocationChange>().shipOnList);
                }

                else
                {

                //TODO: right now the only way a player leaves a ship is in a fighter so if they are on the hangar or the bridge they should still jump
                //debug commented out to use teleporters :  GetComponent<PhotonView>().RPC("NoParent", PhotonTargets.AllBufferedViaServer);
            }
            transform.position = col.gameObject.GetComponent<LocationChange>().exit.transform.position;
                transform.rotation = col.gameObject.GetComponent<LocationChange>().exit.transform.rotation;
                
            }
      //  }

    }

 

    public void OnCollisionEnter(Collision col2)
    {
        if (col2.gameObject.tag == "Bullet" && controlled == true)
        {
            GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, 0);
        }
    }

    [PunRPC]
    public void TakeDamage(int dmg, int fromWho)
    {
        
            hp -= dmg;
            if (hpHud.text.Length > 1 && controlled == true)
            {
                hpHud.text = hpHud.text.Remove(hpHud.text.Length - 1);
                // hpHud.text.Remove(-1);
            }
            if (hp <= 0)
            {
                flying = false;

                this.gameObject.active = true;
                if (station != null)
                {

                    station.GetComponent<PhotonView>().RPC("MakeAvailable", PhotonTargets.AllBufferedViaServer);
                }
                if (localPlayer != null)
                {
                    hpHud.text = "i";
                    myCamera.active = true;
                    localPlayer.GetComponent<PlayerMain>().CharacterDied();
                    //localPlayer.GetComponent<PlayerMain>().roundManager.GetComponent<RoundManager>().wasFrakked = true;
                }
                galactica.GetComponent<Galactica>().medbay.GetComponent<Medbay>().PlaceInBed(this.gameObject);
                shipGroup = 1;
                hp = 1;

            }

        
    }


    [PunRPC]
    public void ParentToShip(int newShipParent) { transform.parent = masterShipList.GetComponent<MasterShipList>().ParentHumanToShip(newShipParent).transform; }
    [PunRPC]
    public void NoParent() { transform.parent = null; }

    public void TakeOff()
    {

        //myCamera.active = false;
        GetComponent<PhotonView>().RPC("GetInShip", PhotonTargets.AllBufferedViaServer);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.isWriting)
        //{

        //      stream.SendNext(transform.rotation);
        //     stream.SendNext(transform.position);
        //}
        //else
        //{
        //     transform.rotation = (Quaternion)stream.ReceiveNext();
        //     transform.position = (Vector3)stream.ReceiveNext();
        //}
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
        GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ammo += ammoChange;
       // ammo += ammoChange;
        if (controlled == true)
        {
            ammoHud.text = GetComponent<HumanControls>().heldItem.GetComponent<HeldItem>().ammo.ToString();
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
        if (controlled == true && hp > 0)
        {
            while (hpHud.text.Length < hp)
            { hpHud.text += "i"; }
        }
    }

}
