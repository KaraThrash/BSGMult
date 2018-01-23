using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStation : Photon.PunBehaviour
{
    PhotonView m_PhotonView;
    public GameObject onOffObject;
    public bool on;
    public GameObject myCam;
    public GameObject myStation;
    public GameObject cameraLocation;
    public GameObject user;
    public bool dontChangeCamera;
    public bool canRepair;
    public bool canSabotage;

    public bool damaged;
    public GameObject damageObject; //visual model to show terminal cant be used until repaired
    //TODO: types of computers
    //>> spawns, builds, actively use >> guns,dradis,communications
    // Use this for initialization
    void Start()
    {
        //myCam = GameObject.Find("GalacticaCamera");
        m_PhotonView = GetComponent<PhotonView>();
        

    }
    // Update is called once per frame
    void Update()
    {
        if (user != null) { if (Vector3.Distance(user.transform.position, transform.position) > 5) { GetComponent<PhotonView>().RPC("MakeAvailable", PhotonTargets.AllViaServer); } }
        
        if (on == true) { onOffObject.active = true; } else { onOffObject.active = false; }
    }
    [PunRPC]
    public void ToggleOnOff()
    {
        //TODO: in use or not in use
       
        if (on == false)
        {
            on = true; onOffObject.active = true;


        }
        else { on = false; onOffObject.active = false; }

    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject == user) {
            on = false;
            onOffObject.active = false;

        }
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(food);
            stream.SendNext(on);
        }
        else
        {
            // food = (int)stream.ReceiveNext();
            on = (bool)stream.ReceiveNext();
        }
    }
    public void Interact(GameObject whoUsedMe)
    {
        if (on == false && damaged == false) {
            
            whoUsedMe.GetComponent<HumanControls>().canMove = false;
            whoUsedMe.GetComponent<PlayerCharacter>().station = this.gameObject;
            GetComponent<PhotonView>().RPC("ToggleOnOff", PhotonTargets.AllViaServer);
            user = whoUsedMe;
            if (dontChangeCamera == false)
            {
                whoUsedMe.GetComponent<PlayerCharacter>().myCamera.active = false;
            }
            myStation.SendMessage("Manned");
         
        }
        

    }
    [PunRPC]
    public void Damaged()
    {
        damaged = true;
        damageObject.active = true;
        if (user != null) { user.GetComponent<PlayerCharacter>().myCamera.active = true; user.GetComponent<HumanControls>().canMove = true; user.GetComponent<PlayerCharacter>().station = null; } 
        on = false;
        onOffObject.active = false;
        user = null;
        if (myStation != null) { myStation.SendMessage("NotManned"); }

    }
    public void Repair(GameObject whoUsedMe)
    {
        if (canRepair == true)
        { myStation.SendMessage("Repair"); }
        damaged = false;
        damageObject.active = false;
    }

    public void Sabotage(GameObject whoUsedMe)
    {
        if (canSabotage == true)
        { myStation.SendMessage("Sabotage"); }
       
    }


    [PunRPC]
    public void MakeAvailable() {
        on = false;
        onOffObject.active = false;
        user = null;
        if (myStation != null) { myStation.SendMessage("NotManned"); }
        
    }
}