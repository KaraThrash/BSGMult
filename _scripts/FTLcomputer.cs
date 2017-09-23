using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FTLcomputer : Photon.PunBehaviour
{
    public int xCord = 1;
    public int yCord = 0;
    public int zCord = 0;
    public GameObject spaceCoordinates;
    public GameObject theFleet;
    public GameObject jumpTarget;
    public int jumpTargetCoordinates;
    public int currentCoordinates;
    public int previousJumpCoordinates;
    public GameObject jumpTargetLocation;
    public Vector3 previousLocation;
    public Text jumpCalculations;
    public float timeSpentCalculating;
    public GameObject myShip;
    public GameObject myHangar;
    public float interactTimer;
    public float timeCost;
    public GameObject jumpTargetOne;
    public Text oneStats;
    public GameObject jumpTargetTwo;
    public Text twoStats;
    public GameObject jumpTargetThree;
    public Text threeStats;
    public GameObject jumpTargetFour;
    public Text fourStats;
    public GameObject jumpTargetFive;
    public Text fiveStats;
    public GameObject jumpTargetSix;
    public Text sixStats;


    public GameObject jumpTargetTextObject;
    public int fuelCost;
    public int distance;
    public int risk;
    public GameObject mappedLocations;
    public GameObject unknownLocations;
    public int numberOfJumps;
    public bool fleetCanSee;

    // Use this for initialization
    void Start() {
        spaceCoordinates = GameObject.Find("Space");
        SetAvailableJumpCoordinates();
        jumpTargetTextObject.transform.parent = GameObject.Find("Canvas").transform;
    }

    // Update is called once per frame
    void Update() {
        //if ( )
        // { GetComponent<PhotonView>().RPC("SetFtlTarget", PhotonTargets.AllViaServer); }
        if (Input.GetKey(KeyCode.Tab)) { jumpTargetTextObject.active = true;


        } else { jumpTargetTextObject.active = false; }


    }

    public void Interact(GameObject whoUsedMe)
    {
        interactTimer += Time.deltaTime;
        if (interactTimer >= timeCost) { interactTimer = 0; CalculateJump();
        }

    }

    //sets new space coordinate locations
    public void SetAvailableJumpCoordinates() {
        //TODO: note plotted locations only appear after 1 jump is calculated. So when you jump in you have no immediatly available jump locations.
        //after you chart the first one some available jumps become visible at no extra cost
        jumpTargetOne = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord + 1, yCord , zCord);
        jumpTargetTwo = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord - 1, yCord , zCord);
        jumpTargetThree = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord , yCord , zCord + 1);
        jumpTargetFour = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord , yCord, zCord - 1);
        jumpTargetFive = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord , yCord + 1, zCord );
        jumpTargetSix = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord , yCord - 1, zCord );

    }

    //Map the available jump targets
    [PunRPC]
    public void SetFtlTarget() {
        if (jumpTargetOne.GetComponent<FTLTarget>().mapped == true)
        {
            oneStats.gameObject.active = true;
            oneStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            
        }
         if (jumpTargetTwo.GetComponent<FTLTarget>().mapped == true)
        {
            twoStats.gameObject.active = true;
            twoStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
        }
         if (jumpTargetThree.GetComponent<FTLTarget>().mapped == true)
        {
            threeStats.gameObject.active = true;
            threeStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
        }
         if (jumpTargetFour.GetComponent<FTLTarget>().mapped == true)
        {
            fourStats.gameObject.active = true;
            fourStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
        }
         if (jumpTargetFive.GetComponent<FTLTarget>().mapped == true)
        {
            fiveStats.gameObject.active = true;
            fiveStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
        }
        
        if(jumpTargetSix.GetComponent<FTLTarget>().mapped == true)
        {
            sixStats.gameObject.active = true;
            sixStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
        }

        if (jumpTargetOne.GetComponent<FTLTarget>().mapped == false) { jumpTargetOne.GetComponent<PhotonView>().RPC("Map", PhotonTargets.AllViaServer);
            oneStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            oneStats.gameObject.active = true;
        }
        else if (jumpTargetTwo.GetComponent<FTLTarget>().mapped == false) { jumpTargetTwo.GetComponent<PhotonView>().RPC("Map", PhotonTargets.AllViaServer);
            twoStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            twoStats.gameObject.active = true;
        }
        else if (jumpTargetThree.GetComponent<FTLTarget>().mapped == false) { jumpTargetThree.GetComponent<PhotonView>().RPC("Map", PhotonTargets.AllViaServer);
            threeStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            threeStats.gameObject.active = true;
        }
        else if (jumpTargetFour.GetComponent<FTLTarget>().mapped == false) { jumpTargetFour.GetComponent<PhotonView>().RPC("Map", PhotonTargets.AllViaServer);
            fourStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            fourStats.gameObject.active = true;
        }
        else if (jumpTargetFive.GetComponent<FTLTarget>().mapped == false) { jumpTargetFive.GetComponent<PhotonView>().RPC("Map", PhotonTargets.AllViaServer);
            fiveStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            fiveStats.gameObject.active = true;
        }
        else { jumpTargetSix.GetComponent<PhotonView>().RPC("Map", PhotonTargets.AllViaServer);
            sixStats.text = jumpTargetOne.GetComponent<FTLTarget>().locationType;
            sixStats.gameObject.active = true;
        }



    }

    public void CalculateJump()
    {

        GetComponent<PhotonView>().RPC("SetFtlTarget", PhotonTargets.AllViaServer);
    }

    //Jump

    //TODO: if the admiral changed hands after this is selected but before the jump would this leave a space location just floating around?
    public void SelectThisJUmpDestination(int jumpTargetSelected)
    {
        //OnButton

        GameObject temp2 = JumpTargetRelativeCoordinates(jumpTargetSelected);
        if (temp2.GetComponent<FTLTarget>().mapped == true)
        {

            //int tempInt = int.Parse(temp2.transform.name);
            
            GetComponent<PhotonView>().RPC("SetJumpTargetForServer", PhotonTargets.AllViaServer, temp2.transform.name);
     
        }
    }
    public GameObject JumpTargetRelativeCoordinates(int relativeCoord) {
        GameObject tempJumpObject;
        switch (relativeCoord)
        {
            case 2:
                tempJumpObject = jumpTargetTwo;
                break;
            case 3:
                tempJumpObject = jumpTargetThree;
                break;
            case 4:
                tempJumpObject = jumpTargetFour;
                break;
            case 5:
                tempJumpObject = jumpTargetFive;
                break;
            case 6:
                tempJumpObject = jumpTargetSix;
                break;
            default:
                tempJumpObject = jumpTargetOne;
                break;
        }
        return tempJumpObject;
    }
    [PunRPC]
    public void SetJumpTargetForServer(string jumpTargetVariable) {
        //TODO: find out why there is this extra 48
        char[] myChars = jumpTargetVariable.ToCharArray();

        xCord = myChars[0] - 48;

        yCord = myChars[1] - 48;
        zCord = myChars[2] - 48;
        jumpTarget = GameObject.Find(jumpTargetVariable);
     
        int tempInt = int.Parse(jumpTargetVariable);
        jumpTargetCoordinates = tempInt;
       // jumpTarget = spaceCoordinates.GetComponent<SpaceCoordinates>().SelectSpace(xCord + 1, yCord, zCord);
        
    }

    public void JumpTheFleet() {
        myShip.GetComponent<Galactica>().theFleet.GetComponent<PhotonView>().RPC("Jumping", PhotonTargets.AllViaServer, jumpTargetCoordinates);
        //myShip.GetComponent<Galactica>().theFleet.GetComponent<Fleet>().Jumping();
    }

    public void JumpThisShip() {
        if (jumpTarget != null)
        { GetComponent<PhotonView>().RPC("SelectJumpDestinationOnServer", PhotonTargets.AllViaServer , jumpTargetCoordinates);
            
        }
        }
    [PunRPC]
    public void SelectJumpDestinationOnServer(int coords)
    {
        numberOfJumps++;
        myShip.GetComponent<PhotonView>().RPC("Jump", PhotonTargets.AllBufferedViaServer, jumpTargetCoordinates);
       // myShip.GetComponent<PhotonView>().RPC("Jump", PhotonTargets.AllBufferedViaServer);
        myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllBufferedViaServer);
        SetAvailableJumpCoordinates();
        //jumpTarget = null;

      
            oneStats.text = "Jump not calculated";

        twoStats.text = "Jump not calculated";

        threeStats.text = "Jump not calculated";
        fourStats.text = "Jump not calculated";

        fiveStats.text = "Jump not calculated";

        oneStats.text = "Jump not calculated";
        oneStats.gameObject.active = false;
        twoStats.gameObject.active = false;
        threeStats.gameObject.active = false;
        fourStats.gameObject.active = false;
        fiveStats.gameObject.active = false;
        sixStats.gameObject.active = false;
        

    }

    public void CommandFleetToJump() { }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

             // stream.SendNext(jumpTarget);
            //  stream.SendNext(rotationObject.transform.rotation);
        }
        else
        {
            // fwdObject.transform.position = (Vector3)stream.ReceiveNext();
            //jumpTarget  = (GameObject)stream.ReceiveNext();
        }
    }

}
