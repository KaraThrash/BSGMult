using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public bool open;
    public Vector3 openPosition;
    public Vector3 closedPosition;
	// Use this for initialization
	void Start () {
        transform.localEulerAngles = openPosition;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact(GameObject whoUsedMe)
    {

        if (open == true)
        {
            open = false;
            transform.localEulerAngles = openPosition;
        }
        else {
            open = true;
            transform.localEulerAngles = closedPosition;
        }

    }
    public void Repair(GameObject whoUsedMe)
    {
      

    }

    public void Sabotage(GameObject whoUsedMe)
    {
     

    }
}
