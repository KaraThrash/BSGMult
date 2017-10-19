using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterWing : Photon.PunBehaviour
{
    public List<GameObject> ships = new List<GameObject>();
    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    [PunRPC]
    public void JustSpawned()
    {
        if (GameObject.Find("RaiderParentObject").transform != null)
        {
            transform.parent = GameObject.Find("RaiderParentObject").transform;

            foreach (Transform raiderchild in transform)
            { raiderchild.GetComponent<Raider>().AssignPatrol(); }
            this.gameObject.active = true;
        }
    }
}
