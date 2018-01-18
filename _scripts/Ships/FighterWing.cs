using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWing : Photon.PunBehaviour
{
    public List<GameObject> ships = new List<GameObject>();
    public int faction;
    public int targetMaxSize; //compared against dradis object size. Can it target capital ships of fleet ships


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
    public GameObject roundManager;
    //TODO: create formation types for different type of ships/targets

    // Use this for initialization
    void Start()
    {

         roundManager = GameObject.Find("RoundManager");
        //GameObject raiderParent = GameObject.Find("ActiveCylonFleet");
        //if (raiderParent != null)
        //{
        //    transform.parent = raiderParent.transform;
        //}
        AssignPatrol();
        shipCount = ships.Count;
    }

    // Update is called once per frame
    void Update() {
        if (roundManager == null)
        {
            roundManager = GameObject.Find("RoundManager");
        }
        if (shipTarget == null) { Patrol(); }
            if (shipTarget != null) { Attack(); if (Vector3.Distance(transform.position, shipTarget.transform.position) > 4000) { shipTarget = null; } }
        
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

        transform.position = Vector3.MoveTowards(transform.position, shipTarget.transform.position, speed * Time.deltaTime);

        
        //    //TODO: make AI vipers do what the pegasus vipers do in the fight vs galactica. the rigid hard diretion turn then forward
       
    }
    public void Patrol()
    {
        canPatrol = true;
        if (patrolTarget != null)
        {
            targetRotation = Quaternion.LookRotation(patrolTarget.transform.position - transform.position);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 3 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, patrolTarget.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(patrolTarget.transform.position, transform.position) < 10) { GotoNextPoint(); }
        }
    }
    void GotoNextPoint()
    {
        if (points.Length == 0)
        { return; }
        currentPoint = (currentPoint + 1) % points.Length;
        patrolTarget = points[currentPoint];

    }
    public void OnTriggerStay(Collider other)
    {
        // Debug.Log(other.transform.name);
        //TODO: fix border to just be an actual border instead of full sphere
        if (shipTarget == null)
        {
            if (other.GetComponent<Dradis>() != null)
            {
                if (other.GetComponent<Dradis>().dradisValue <= targetMaxSize && other.GetComponent<Dradis>().faction != faction)
                { shipTarget = other.GetComponent<Dradis>().myShip; }
            }
            
           
           // if (other.tag == "Viper") { if (shipTarget == null && other.transform.parent.gameObject.GetComponent<Fighter>().flying == true) { shipTarget = other.transform.parent.gameObject; } }
           // if (other.tag == "Fleetship") { if (shipTarget == null) { shipTarget = other.transform.parent.gameObject; } }

        }
        
    }

    [PunRPC]
    public void ShipDestroyed(int shipNumber, int byWho)
    {
        if (ships.Count >= shipNumber )
        {
            shipsDestroyed++;
            //Destroy(ships[shipNumber]);
            if (ships[shipNumber] != null )
            {
                roundManager.GetComponent<RoundManager>().CylonKilled(1, byWho);
                ships[shipNumber].active = false;
               // if (shipsDestroyed >= ships.Count) { Destroy(this.gameObject); }

            }
            //shipCount--;
           
            // ships.RemoveAt(shipNumber);
        }
        
        
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
