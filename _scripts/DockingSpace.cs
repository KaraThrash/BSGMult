using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingSpace : MonoBehaviour {
    public GameObject shipParked;
    public bool spaceOpen;
    public GameObject myHangar;
    public GameObject myLaunchBay;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject == shipParked)
        {
            spaceOpen = true;
            myHangar.GetComponent<LandingBay>().ShipLeavesHangar(shipParked);
            shipParked = null;
            
        }
    }
}
