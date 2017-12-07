using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCoordinates : MonoBehaviour {

    public bool mapped;
    public int spaceLocation;
    public string locationType;
    public int thisX;
    public int thisY;
    public int thisZ;
    public int maxX;
    public int maxY;
    public int maxZ;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public int SelectSpace(int newX,int newY, int newZ) {
        mapped = false;
        if (newX > maxX) { newX = 1; locationType = "Planet"; }
        if (newX < 1) { newX = maxX; locationType = "OpenSpace"; }
        if (newY > maxY) { newY = 0; }
        if (newY < 0) { newY = maxY; }
        if (newZ > maxZ) { newZ = 0; }
        if (newZ < 0) { newZ = maxZ; }

        if (newX % 2 == 0) { locationType = "Planet"; } else { locationType = "OpenSpace"; }
        thisX = newX;
        thisY = newY;
        thisZ = newZ;


       // Debug.Log(newX.ToString() + " " + newY.ToString() + " " + newZ.ToString());
        // string xstr = 
        string newCoordinates = newX.ToString() + newY.ToString() + newZ.ToString();
         spaceLocation = (thisX * 100) + (thisY * 10) + thisZ;
        //TODO: use this to compare to a formula you make later that determines the type of location the charted area witll be

        return spaceLocation;
    }


}
