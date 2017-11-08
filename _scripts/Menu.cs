﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject menuButtons;
    public GameObject scoreObj;
    public GameObject objectiveObj;
    public GameObject resourceObj;
    public GameObject jumpObj;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
                menuButtons.active = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreObj.active = false;
            objectiveObj.active = false;
            resourceObj.active = false;
            jumpObj.active = false;
            menuButtons.active = false;
        }
    }
    public void MenuSelect(int menutab)
    {
        //todo: on menu open have permissions based on character rank/faction
        scoreObj.active = false;
        objectiveObj.active = false;
        resourceObj.active = false;
        jumpObj.active = false;
        switch (menutab)
        {
            case 1:
                scoreObj.active = true;
                break;
            case 2:
                resourceObj.active = true;
                break;
            case 3:
                jumpObj.active = true;
                break;

            default:
                objectiveObj.active = true;
                break;
        }

    }

    public void Scoreboard()
    { }
    public void ObjectiveBoard()
    { }
    public void ResourceBoard()
    { }
    public void JumpBoard()
    { }
}