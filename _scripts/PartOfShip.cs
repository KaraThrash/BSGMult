using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfShip : MonoBehaviour {
    public GameObject myShip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            myShip.SendMessage("TakeDamage");
            Debug.Log("hit");
        }
    }
}
