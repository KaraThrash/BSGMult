using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidMovement : Photon.PunBehaviour
{
    PhotonView m_PhotonView;
    public int runSpeed;
    public GameObject cam;
    public bool grounded;
    public float airTime;
    public float groundCheckDistance;
    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        m_PhotonView = GetComponent<PhotonView>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        if (m_PhotonView.isMine == true)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) { Move(); }

            CheckGround();
        }
    }
    public void Move()
    {


        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //if (dir != Vector3.zero)
        //{
        //    transform.rotation = Quaternion.Slerp(
        //        transform.rotation,
        //        Quaternion.LookRotation(dir),
        //        Time.deltaTime * 10
        //    );
        //}
        Vector3 forward = cam.transform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;


        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        Vector3 targetDirection;

        targetDirection = forward * v + right * h;
        transform.localRotation = Quaternion.Slerp(
               transform.rotation,
               Quaternion.LookRotation(targetDirection),
               Time.deltaTime * 10
           );
       // transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y,0);
        //if (grounded == true && rb.velocity.magnitude < 12)
        //{

        //    rb.AddForce(transform.forward * 20 * Time.deltaTime, ForceMode.Impulse);
        //}
        //else { rb.AddForce(transform.forward * 10  * Time.deltaTime, ForceMode.Impulse); }
        rb.AddForce(transform.forward * 10 * v * Time.deltaTime, ForceMode.Impulse);
        rb.AddForce(transform.right * 10 * h * Time.deltaTime, ForceMode.Impulse);
    }
    public void CheckGround()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.down);
        //Physics.Raycast(transform.position, fwd, groundCheckRange) ||
        if (Physics.Raycast(transform.position, fwd, groundCheckDistance))
        {
            grounded = true;
            airTime = 0;
        }
        else
        {
            grounded = false;
            rb.AddForce(transform.up * -5 * Time.deltaTime, ForceMode.Impulse);
            if (airTime < 20) { airTime += 0.1f; }

        }
    }

}
