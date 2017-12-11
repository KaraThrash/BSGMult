using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckGun : MonoBehaviour {
    public bool manned;
    public GameObject myTerminal;
    public float mouseX;
    public float mouseY;
    public float gunCoolDown;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public GameObject turretHead;
    public GameObject readyPosition;
    public GameObject unmannedPosition;
    public GameObject mannedPosition;
    public Quaternion targetRotation;
    public GameObject myCamera;
    public float focusPointX;
    public bool activated;
    public int leftLimit;
    public int upLimit;
    public int rightLimit;
    public int downLimit;
    public float hortTarget = 20.0f;
    public float vertTarget = 20.0f;
    public Vector3 rotateTo;
    public int rotateDirection;
    public bool leftSide;
    public int hp;
    public int maxHp;
    public float rotSpeed; //reversed if on left side
    public int leftOrRight; //left -1
    //private Rigidbody rb;
    // Use this for initialization
    void Start () {
        // rb = GetComponent<Rigidbody>();
      
    }
	
	// Update is called once per frame
	void Update () {
        if(hp > 0) { 
        
            if (manned == true)
            {
                turretHead.transform.localEulerAngles = new Vector3(hortTarget, vertTarget, 0);
                gunCoolDown -= Time.deltaTime;
                if (Input.GetMouseButton(0))
                {
                    if (gunCoolDown <= 0)
                    {
                        GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                        gunCoolDown = 0.6f;
                    }

                }

                
                AimGun();
            }

             else { turretHead.transform.localEulerAngles = Vector3.Slerp(turretHead.transform.localEulerAngles, new Vector3(hortTarget, vertTarget, 0), 8.0f * Time.deltaTime); }
            if (Input.GetKey(KeyCode.Backspace)) { manned = false; myCamera.active = false; }
        }
	}
    [PunRPC]
    public void ShootGuns()
    {
        Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
        Instantiate(bullet, gun1.transform.position, gun1.transform.rotation);

    }
    public void AimGun() {

        // transform.Rotate(Vector3.right * Time.deltaTime);
        Debug.Log(turretHead.transform.localEulerAngles);
        if (Input.GetKey(KeyCode.W) && turretHead.transform.localEulerAngles.x < upLimit) { turretHead.transform.Rotate(transform.forward * rotSpeed * leftOrRight * Time.deltaTime, Space.Self); }
        if (Input.GetKey(KeyCode.S) && turretHead.transform.localEulerAngles.x > downLimit) { turretHead.transform.Rotate(transform.forward * -rotSpeed * leftOrRight * Time.deltaTime, Space.Self); }
        if (Input.GetKey(KeyCode.D) && turretHead.transform.localEulerAngles.y < rightLimit) { turretHead.transform.Rotate(transform.up * rotSpeed   * Time.deltaTime, Space.Self); }
        if (Input.GetKey(KeyCode.A) && turretHead.transform.localEulerAngles.y  > leftLimit) { turretHead.transform.Rotate(transform.up * -rotSpeed  * Time.deltaTime,Space.Self); }
        vertTarget = turretHead.transform.localEulerAngles.y;
        hortTarget = turretHead.transform.localEulerAngles.x;
        GetComponent<PhotonView>().RPC("SyncAimTarget", PhotonTargets.Others, hortTarget, vertTarget);
      
    }
    public void Manned() {
        if (hp > 0 && activated == false)
        {
            myCamera.active = true;
            manned = true;
            activated = true;
            GetComponent<PhotonView>().RPC("ActivateOnServer", PhotonTargets.AllBufferedViaServer);
        }
    }
    [PunRPC]
    public void SyncAimTarget(float newH,float newV) {
        vertTarget = newV;
        hortTarget = newH;

        //activated = true;
    }


    [PunRPC]
    public void ActivateOnServer() { activated = true; }

  
    public void TakeDamage(int dmg)
    {
        if (hp > 0)
        {
            hp -= dmg;

            if (hp <= 0 )
            {
                hp = 0;
                // myTerminal.GetComponent<BattleStation>().damageObject.active = true;
                NotManned();
                myTerminal.GetComponent<PhotonView>().RPC("Damaged", PhotonTargets.AllViaServer);
            }
          
            Debug.Log("hit");
        }
    }


    public void Repair()
    {
        if (hp < maxHp)
        {
            hp++;

        }
    }

    public void Sabotage()
    {
        if (hp > 0)
        {
            hp--;

        }
    }
    public void NotManned()
    {
        myCamera.active = false;
        manned = false;
        activated = false;
        GetComponent<PhotonView>().RPC("SyncAimTarget", PhotonTargets.AllViaServer, 20.0f  , 20.0f);
        
        //GetComponent<PhotonView>().RPC("ActivateOnServer", PhotonTargets.AllBufferedViaServer);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(activated);
            stream.SendNext(turretHead.transform.rotation);
        }
        else
        {
            activated = (bool)stream.ReceiveNext();
          //  readyPosition.transform.position = (Vector3)stream.ReceiveNext();
            turretHead.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
