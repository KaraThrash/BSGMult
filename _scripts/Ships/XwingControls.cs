using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XwingControls : Photon.PunBehaviour
{

    public GameObject player;
    public GameObject myCamera;
    public float XSensitivity = 1.5f;
    public float YSensitivity = 1.5f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public float roll;
    public float rollSpeed;
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    public int playerNumber;
    public bool flying;
    PhotonView m_PhotonView;
    private Rigidbody rb;
    public float lift;
    public bool exit;
    public float hort;
    public float vert;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bullet;
    public float gunCoolDown;
    public bool playerControlled;
    public float afterBurner;
    private Vector3 holdPosValue;
    public int turnSpeed;
    public int flySpeed;
    public int liftSpeed;
    public int strafeSpeed;
    public bool controlled;
    public float range;


    public void Start()
    {
        range = 1000.0f;
        rb = GetComponent<Rigidbody>();
        m_PhotonView = GetComponent<PhotonView>();

        m_CharacterTargetRot = myCamera.transform.localRotation;
        m_CameraTargetRot = myCamera.transform.localRotation;
    }

    public void Update()
    {
        if (flying != GetComponent<Fighter>().flying)
        { flying = GetComponent<Fighter>().flying; }

        if (flying == true)
        {
            //Secondary check to make sure the ship isnt active when docked
            if (transform.position.y < -500 && transform.position.y > -800)
            {

                GetComponent<Fighter>().OutOfBounds();

            }
            if (m_PhotonView.isMine == true)
            {

                MouseFlightControls(); 

            }
            else { hort = 0; vert = 0; }

        }
        if (gunCoolDown > 0) { gunCoolDown -= Time.deltaTime; }
      


    }
    public void MouseFlightControls()
    {
        if (Input.GetKey(KeyCode.Tab))
        {

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                if (gunCoolDown <= 0 && rb.velocity.magnitude < 250)
                {
                  
                    RaycastShootGuns();
                    GetComponent<PhotonView>().RPC("ShootGuns", PhotonTargets.AllViaServer, GetComponent<Rigidbody>().velocity);
                    gunCoolDown = 0.1f;
                }
                gunCoolDown = gunCoolDown - Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.T) && flying == true)
            {
                GetComponent<Fighter>().Land();

            }
            if (Input.GetKey(KeyCode.Space)) { lift = liftSpeed; } else if (Input.GetKey(KeyCode.LeftShift)) { lift = -liftSpeed; } else { lift = 0; }
            hort = Input.GetAxis("Horizontal");
            vert = Input.GetAxis("Vertical");

            flightControls(vert, hort, 0, 0, 0, exit, lift);

       
            Cursor.visible = false;

            Cursor.lockState = CursorLockMode.Locked;
            LookRotation(player.transform, this.transform);
        }
    }
    public void flightControls(float newvert, float newhort, float roll, float rollX, float rollY, bool leave, float lift)
    {
        if (vert != 0)
        {
            rb.AddForce(transform.forward * ((-vert * flySpeed * afterBurner)) * Time.deltaTime);
        }
        if (hort != 0)
        {
            rb.AddForce(transform.right * (-hort * strafeSpeed) * Time.deltaTime, ForceMode.Impulse);
        }

        if (lift != 0) { rb.AddForce(transform.up * lift * 30 * Time.deltaTime); }

    }
    public void RaycastShootGuns()
    {

        RaycastHit hit;


        if (Physics.Raycast(gun1.transform.position, -gun1.transform.forward, out hit, range))
        {

            if (hit.transform.gameObject.tag == "Raider")
            {

                hit.transform.gameObject.GetComponent<Raider>().TakeDamage(1, 1);
            }
            else if (hit.transform.gameObject.tag == "Fighter")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
            }
            else if (hit.transform.gameObject.tag == "TargetableSystem")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
            }
        }

        if (Physics.Raycast(gun2.transform.position, -gun2.transform.forward, out hit, range))
        {

            if (hit.transform.gameObject.tag == "Raider")
            // if (hit.transform.gameObject.GetComponent<Fighter>() != null || hit.transform.gameObject.GetComponent<Dradis>() != null)
            {

                hit.transform.gameObject.GetComponent<Raider>().TakeDamage(1, 1);
                // hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1,GetComponent<Fighter>().playerNumber);
            }
            else if (hit.transform.gameObject.tag == "Fighter")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
                // hit.transform.gameObject.GetComponent<Fighter>().TakeDamage(1, playerNumber);
            }
            else if (hit.transform.gameObject.tag == "TargetableSystem")
            {
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllViaServer, 1, GetComponent<Fighter>().playerNumber);
                //hit.transform.gameObject.GetComponent<PartOfShip>().TakeDamage(1, GetComponent<Fighter>().playerNumber);
            }
        }
    }
    [PunRPC]
    public void ShootGuns(Vector3 currentVelocity)
    {



        GameObject clone = Instantiate(bullet, gun2.transform.position, gun2.transform.rotation) as GameObject;
        clone.GetComponent<Rigidbody>().velocity += currentVelocity;

        GameObject clone2 = Instantiate(bullet, gun1.transform.position, gun1.transform.rotation) as GameObject;
        clone2.GetComponent<Rigidbody>().velocity += currentVelocity;
    }
    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = Input.GetAxis("Mouse X") * XSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * YSensitivity;
        if (Input.GetKey(KeyCode.Q)) { roll = -rollSpeed; } else if (Input.GetKey(KeyCode.E)) { roll = rollSpeed; } else {
            roll = 0;
        }
        m_CharacterTargetRot *= Quaternion.Euler(xRot, yRot, roll);


   

        if (smooth)
        {
            character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);

        }
        else
        {
            character.localRotation = m_CharacterTargetRot;

        }



    }



    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //  m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //  m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            // Cursor.lockState = CursorLockMode.None;
            //  Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}