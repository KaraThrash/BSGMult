using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : Photon.PunBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [PunRPC]
    public void EnableCharacter(int characterNumber)
    {
        if (characters[characterNumber] != null)
        {
            characters[characterNumber].active = true;
        }
    }
}
