using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medbay : MonoBehaviour {
    public List<GameObject> beds = new List<GameObject>();
   
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
       

	}

    public void PlaceInBed(GameObject woundedPerson)
    {
        
        for (int i = beds.Count - 1; i >= 0; --i)
        {
            if (beds[i].GetComponent<HospitalBed>().bedOpen == true)
            {
                woundedPerson.transform.position = beds[i].transform.position;
                beds[i].GetComponent<HospitalBed>().PlaceWoundedPerson(woundedPerson);
                break;
            }
        }
    }

    

}
