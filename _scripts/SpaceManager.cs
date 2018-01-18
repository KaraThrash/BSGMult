using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour {
    public List<GameObject> spaceObjects = new List<GameObject>();
    public GameObject currentSpace;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SpawnSpace(int newSpaceNumber)
    {
        if (currentSpace != null)
        { Destroy(currentSpace); }
        if (newSpaceNumber < 3)
        { currentSpace = Instantiate(spaceObjects[newSpaceNumber], transform.position, transform.rotation); }
        else { currentSpace = Instantiate(spaceObjects[2], transform.position, transform.rotation); }
        currentSpace.transform.parent = this.transform;
    }
}
