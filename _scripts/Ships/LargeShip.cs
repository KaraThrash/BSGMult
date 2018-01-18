using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeShip : Photon.PunBehaviour
{
    public Transform peopleOnBoard;
    public bool fleetShip;
    public bool manned;
    public bool canRotate;
    public float speed;
    public int size; //galactica and basestars being the largest
    public int hp;
    public GameObject myCamera;
    public GameObject fwdObject;
    public GameObject rotationObject;
    public GameObject engineAnim;
    public float rotSpeed = 0.5f;

    public GameObject explosion;
    public GameObject impactExplosion;
    public float impactTimer;
    public Vector3 lastImpact;
    public Quaternion impactRotation;
    public bool destroyed;
    public float dieClock;
    public GameObject breachLocation; //for heavy raiders
    public bool dontDie;
    public float lengthOfDeathAnimation = 5.0f;
    // Use this for initialization
    void Start()
    {
        lastImpact = transform.position * 1.1f;
        impactRotation = transform.rotation ;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyed == true)
        {
            if (impactTimer < 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, lastImpact, -21.1f * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, impactRotation, -0.5f * Time.deltaTime);
            }
            dieClock -= Time.deltaTime;
            if (dieClock <= 0)
            { Die(); }

        }
        
        if (impactTimer > 0)
        {
            impactTimer -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, lastImpact, -15.0f * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, impactRotation, -0.1f * Time.deltaTime);

            rotationObject.transform.rotation = transform.rotation;
        }
        else
        {
            if (canRotate == true)
            { transform.rotation = Quaternion.Slerp(transform.rotation, rotationObject.transform.rotation, 1.0f * Time.deltaTime); }
            
            
            if (Vector3.Distance(fwdObject.transform.position, transform.position) > 10)
            {
                transform.position = Vector3.MoveTowards(transform.position, fwdObject.transform.position, speed);
                engineAnim.active = true;
            }
            else { engineAnim.active = false; }
            if (manned == true)
            {
                BeingFlown();
            }
        }
        fwdObject.transform.position = Vector3.MoveTowards(fwdObject.transform.position, transform.position, 1.0f);
    }
    public void BeingFlown()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<PhotonView>().RPC("SetForwardObject", PhotonTargets.AllViaServer);
            //transform.Translate(Vector3.right * 1);
        }
        
            if (Input.GetKey(KeyCode.S)) { rotationObject.transform.Rotate(0.2f, 0, 0); }
            if (Input.GetKey(KeyCode.W)) { rotationObject.transform.Rotate(-0.2f, 0, 0); }
            if (Input.GetKey(KeyCode.Q)) { rotationObject.transform.Rotate(0, 0, -0.2f); }
            if (Input.GetKey(KeyCode.E)) { rotationObject.transform.Rotate(0, 0, 0.2f); }
            if (Input.GetKey(KeyCode.D)) { rotationObject.transform.Rotate(0, 0.2f, 0); }
            if (Input.GetKey(KeyCode.A)) { rotationObject.transform.Rotate(0, -0.2f, 0); }

           
            GetComponent<PhotonView>().RPC("SetRotationObjects", PhotonTargets.Others, rotationObject.transform.rotation);
        
    }

    [PunRPC]
    public void SetRotationObjects(Quaternion rot) { rotationObject.transform.rotation = rot; }
    [PunRPC]
    public void SetForwardObject() {
        if (fwdObject.transform.localPosition.z > -50)
        {
            fwdObject.transform.localPosition = new Vector3(0, 0, fwdObject.transform.localPosition.z - 5);
        }
    }

    public void Manned() {
        rotationObject.transform.rotation = transform.rotation;
        manned = true;
        myCamera.GetComponent<RPGCamera>().Target = this.transform;
        myCamera.active = true;
    }
    public void NotManned() {
        manned = false;
        myCamera.active = false;
        rotationObject.transform.rotation = transform.rotation;
    }

    public void TakeDamage(int dmgTaken)
    {
        if (hp > 0)
        {
            hp -= dmgTaken;

            if (hp <= 0 && destroyed == false)
            {
                GetComponent<PhotonView>().RPC("DieOnServer", PhotonTargets.AllViaServer);
                
                
              
            }
            //Destroy(this.gameObject);
            Debug.Log("hit");
        }
    }
    [PunRPC]
    public void DieOnServer()
    {
        GameObject clone = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        clone.transform.parent = this.transform;

        destroyed = true;
        hp = 0;
        dieClock = lengthOfDeathAnimation;
    }
    public void Die()
    {
        if (fleetShip == true)
        {
            //Die();
            
            this.GetComponent<FleetShip>().FleetShipDie();
            
            //GetComponent<PhotonView>().RPC("Die", PhotonTargets.AllBufferedViaServer);
        }

        if (dontDie == false)
        { ForPassengersAfterDestroyed(); Destroy(this.gameObject); }
        else {
            destroyed = false;
            dieClock = 13.0f;
            Instantiate(explosion, transform.position, transform.rotation);
            hp = 25; } 
       
    }

    public void ForPassengersAfterDestroyed()
    {
        if (peopleOnBoard != null)
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

    public void Repair(GameObject whoUsedMe)
    {
      

    }

    public void Sabotage(GameObject whoUsedMe)
    {
       

    }

    public void Impact(Vector3 pointOfImpact)
    {
        if (impactTimer < 3)
        {
            Instantiate(impactExplosion, transform.position, transform.rotation);
            lastImpact = pointOfImpact;


            impactRotation = Quaternion.LookRotation(pointOfImpact - transform.position);



            impactTimer = 5;
        }


    }
    public void ExitImpact()
    {
        if (impactTimer > 0.5f)
        { impactTimer = 1.5f; }
       

    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            
                stream.SendNext(transform.rotation);
                stream.SendNext(transform.position);
            
        }
        else
        {
             transform.rotation = (Quaternion)stream.ReceiveNext();
             transform.position = (Vector3)stream.ReceiveNext();
 
        }
    }
}