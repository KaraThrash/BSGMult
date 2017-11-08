using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityDetector : MonoBehaviour {

    public int peopleInRange;
    public GameObject enableThis;
    public GameObject enableThisAlso;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<PlayerCharacter>() != null)
        {
            peopleInRange++;


            if (col.GetComponent<PlayerCharacter>().controlled == true)
            {
                enableThis.active = true;
                if (enableThisAlso != null) { enableThisAlso.active = true; }
            }
        }
    }
    public void OnTriggerExit(Collider col2)
    {
        if (col2.GetComponent<PlayerCharacter>() != null)
        {
            peopleInRange--;
        }
        if (peopleInRange <= 0)
        { enableThis.active = false;
            if (enableThisAlso != null) { enableThisAlso.active = false; }
        }
    }
}
