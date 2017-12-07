using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour {
    public List<GameObject> supplyCrates = new List<GameObject>();
    public List<GameObject> weapons = new List<GameObject>();

    public GameObject foodCrate;
    public GameObject fuelCrate;
    public GameObject ammoCrate;

    public GameObject rifle;
    public GameObject wrench;

    public GameObject bomb;
    public GameObject repairKit;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void DropCrate(int itemToDrop,GameObject whereToDrop)
    {
        if (supplyCrates[itemToDrop] != null)
        {
            
            GameObject clone = Instantiate(supplyCrates[itemToDrop], whereToDrop.transform.position, whereToDrop.transform.rotation) as GameObject;
            clone.GetComponent<Rigidbody>().AddForce(whereToDrop.transform.forward * 10, ForceMode.Impulse);
        }
       

    }
}
