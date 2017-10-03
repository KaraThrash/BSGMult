using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckGun : MonoBehaviour {
    public bool manned;
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
    public float hortTarget;
    public float vertTarget;
    public Vector3 rotateTo;
    public int rotateDirection;
    public bool leftSide;
    //private Rigidbody rb;
    // Use this for initialization
    void Start () {
        // rb = GetComponent<Rigidbody>();
        focusPointX = readyPosition.transform.localPosition.x;
    }
	
	// Update is called once per frame
	void Update () {
        if (activated == true)
        {
            turretHead.transform.position = Vector3.MoveTowards(turretHead.transform.position, mannedPosition.transform.position, 5.0f * Time.deltaTime);
           // targetRotation = Quaternion.LookRotation(readyPosition.transform.localPosition - turretHead.transform.localPosition);
            // turretHead.transform.localRotation = Quaternion.Lerp(turretHead.transform.localRotation, targetRotation, 1.0f * Time.deltaTime);
           // turretHead.transform.localEulerAngles = Vector3.Lerp(turretHead.transform.localEulerAngles, new Vector3(hortTarget,vertTarget,0), 1.0f * Time.deltaTime);
            turretHead.transform.localEulerAngles = new Vector3(hortTarget, vertTarget, 0);
        }
        else {
            turretHead.transform.position = Vector3.MoveTowards(turretHead.transform.position, unmannedPosition.transform.position, 1.0f * Time.deltaTime);
            turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, new Quaternion(0,0,0,0), 1.0f * Time.deltaTime);
        }
        //if (targetRotation != turretHead.transform.rotation) { turretHead.transform.rotation = Quaternion.Lerp(turretHead.transform.rotation, targetRotation, 5.0f); }
        if (manned == true) {
            gunCoolDown -= Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
                if (gunCoolDown <= 0)
                {
                    GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer);
                    gunCoolDown = 0.6f;
                }
                
            }

            // mouseX = Input.GetAxis("Horizontal");
            // mouseY = Input.GetAxis("Vertical");
           
           // readyPosition.transform.Translate(Vector3.left * Time.deltaTime);
          //  readyPosition.transform.localPosition = Vector3.MoveTowards(readyPosition.transform.localPosition, new Vector3(focusPointX,mouseY, mouseX), 10.0f);
           // transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(mouseY, mouseX, 0), 0.1f  * Time.deltaTime);
            AimGun();
            if (Input.GetKey(KeyCode.Backspace)) { manned = false; myCamera.active = false;  }
        }
	}
    [PunRPC]
    public void ShootGuns()
    {
        Instantiate(bullet, gun2.transform.position, gun2.transform.rotation);
        Instantiate(bullet, gun1.transform.position, gun1.transform.rotation);

    }
    public void AimGun() {
        if (leftSide == true)
        {
            if (Input.GetKey(KeyCode.W) && hortTarget < upLimit) { hortTarget += 1; }
            if (Input.GetKey(KeyCode.S) && hortTarget > downLimit) { hortTarget -= 1; }
            if (Input.GetKey(KeyCode.D) && vertTarget < rightLimit) { vertTarget += 1; }
            if (Input.GetKey(KeyCode.A) && vertTarget > leftLimit) { vertTarget -= 1; }
        }
        else
        {
            if (Input.GetKey(KeyCode.W) && hortTarget > upLimit) { hortTarget -= 1; }
            if (Input.GetKey(KeyCode.S) && hortTarget < downLimit) { hortTarget += 1; }
            if (Input.GetKey(KeyCode.D) && vertTarget < rightLimit) { vertTarget += 1; }
            if (Input.GetKey(KeyCode.A) && vertTarget > leftLimit) { vertTarget -= 1; }
        }

    }
    public void Manned() {
        myCamera.active = true;
        manned = true;
        GetComponent<PhotonView>().RPC("ActivateOnServer", PhotonTargets.AllBufferedViaServer);
    }
    [PunRPC]
    public void ActivateOnServer() { activated = true; }


    public void NotManned()
    {
        myCamera.active = false;
        manned = false;
        activated = false;
        //GetComponent<PhotonView>().RPC("ActivateOnServer", PhotonTargets.AllBufferedViaServer);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(activated);
           // stream.SendNext(readyPosition.transform.position);
           // stream.SendNext(transform.rotation);
        }
        else
        {
            activated = (bool)stream.ReceiveNext();
           // readyPosition.transform.position = (Vector3)stream.ReceiveNext();
           // targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
