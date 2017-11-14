using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWing : Photon.PunBehaviour
{
    public List<GameObject> ships = new List<GameObject>();
    public string raiderParentType = "ActiveCylonFleet"; //to be able to spawn at scoutable locations
    public int shipsDestroyed;
    public int shipCount;
    public int speed;
    public GameObject patrolPointsParent;
    public GameObject[] points;
    public GameObject patrolTarget;

    public GameObject shipTarget;
    public int currentPoint = 0;
    private Quaternion targetRotation;
    public string patrolPointType;
    public bool canPatrol;
    public GameObject dradisModel;

    //TODO: create formation types for different type of ships/targets

    // Use this for initialization
    void Start()
    {
        GameObject raiderParent = GameObject.Find("ActiveCylonFleet");
        if (raiderParent != null)
        {
            transform.parent = raiderParent.transform;
        }
        AssignPatrol();
        shipCount = ships.Count;
    }

    // Update is called once per frame
    void Update() {
        
            if (shipTarget == null) { Patrol(); }
            if (shipTarget != null) { Attack(); if (Vector3.Distance(transform.position, shipTarget.transform.position) > 3000) { shipTarget = null; } }
        
    }

    public void AssignPatrol()
    {

        patrolPointsParent = GameObject.Find(patrolPointType);
        if (patrolPointsParent != null)
        {
            canPatrol = true;
            foreach (Transform child in patrolPointsParent.transform)
            {
                if (currentPoint < points.Length)
                {
                    points[currentPoint] = child.gameObject;
                    currentPoint++;
                }

            }
            patrolTarget = points[0];
            currentPoint = 0;
        }
    }
    public void Attack()
    {

        

        if (Vector3.Distance(shipTarget.transform.position, transform.position) > 600)
        {
            transform.position = Vector3.MoveTowards(transform.position, shipTarget.transform.position, speed * Time.deltaTime);
           // targetRotation = Quaternion.LookRotation(shipTarget.transform.position - transform.position);

           // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 6 * Time.deltaTime);

        }
        else
        {
            //TODO: make AI vipers do what the pegasus vipers do in the fight vs galactica. the rigid hard diretion turn then forward
            transform.position = Vector3.MoveTowards(transform.position, shipTarget.transform.position, 5 * Time.deltaTime);
            
        }
    }
    public void Patrol()
    {
        canPatrol = true;
        targetRotation = Quaternion.LookRotation(patrolTarget.transform.position - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget.transform.position, speed * Time.deltaTime);
        if (Vector3.Distance(patrolTarget.transform.position, transform.position) < 1) { GotoNextPoint(); }
    }
    void GotoNextPoint()
    {
        if (points.Length == 0)
        { return; }
        currentPoint = (currentPoint + 1) % points.Length;
        patrolTarget = points[currentPoint];

    }
    public void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.transform.name);
        //TODO: fix border to just be an actual border instead of full sphere
        if (other.tag == "Viper") { if (shipTarget == null && other.transform.parent.gameObject.GetComponent<Fighter>().flying == true) { shipTarget = other.transform.parent.gameObject; } }
        if (other.tag == "Fleetship") { if (shipTarget == null) { shipTarget = other.transform.parent.gameObject; } }
    }

    [PunRPC]
    public void ShipDestroyed(int shipNumber)
    {
        if (ships.Count >= shipNumber )
        {
            shipsDestroyed++;
            //Destroy(ships[shipNumber]);
            ships[shipNumber].active = false;
           // ships.RemoveAt(shipNumber);
        }
        
        shipCount--;
        if (shipsDestroyed >= ships.Count) { Destroy(this.gameObject); }
    }

    [PunRPC]
    public void JustSpawned()
    {
        //GameObject raiderParent = GameObject.Find("ActiveCylonFleet");
        //if (raiderParent != null)
        //{
        //    transform.parent = GameObject.Find("RaiderParentObject").transform;

        //    foreach (Transform raiderchild in transform)
        //    { raiderchild.GetComponent<Raider>().AssignPatrol(); }
        //    this.gameObject.active = true;
        //}
    }
    void Awake()
    {

       // Debug.Log("basestar In New Scene");
        // DontDestroyOnLoad(this.gameObject);
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(currentPoint);
          //  stream.SendNext(transform.position);
           
        }
        else
        {
            currentPoint = (int)stream.ReceiveNext();
            // transform.rotation = (Quaternion)stream.ReceiveNext();
            // transform.position = (Vector3)stream.ReceiveNext();

        }
    }
}
