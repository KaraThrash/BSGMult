using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCoordinates : MonoBehaviour {


    public GameObject spaceLocation;
    public int maxX;
    public int maxY;
    public int maxZ;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject SelectSpace(int newX,int newY, int newZ) {
        
        if (newX > maxX) { newX = 1; }
        if (newX < 1) { newX = maxX; }
        if (newY > maxY) { newY = 0; }
        if (newY < 0) { newY = maxY; }
        if (newZ > maxZ) { newZ = 0; }
        if (newZ < 0) { newZ = maxZ; }
       // Debug.Log(newX.ToString() + " " + newY.ToString() + " " + newZ.ToString());
        // string xstr = 
        string newCoordinates = newX.ToString() + newY.ToString() + newZ.ToString();
        spaceLocation = GameObject.Find(newCoordinates);

        return spaceLocation;
    }


}
