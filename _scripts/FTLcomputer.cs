using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FTLcomputer : Photon.PunBehaviour
{
    public int xCord = 1;
    public int yCord = 0;
    public int zCord = 0;
    public int jumpTargetCords;
    public GameObject jumpTarget;

    public GameObject spaceCoordinates;
    public GameObject theFleet;

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

    //changhed the game objects to ints
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
        GetComponent<PhotonView>().RPC("SetAvailableJumpCoordinates", PhotonTargets.AllBufferedViaServer, xCord, yCord, zCord);
    }

    // Update is called once per frame
    void Update() {

        //if (Input.GetKey(KeyCode.Tab)) { jumpTargetTextObject.active = true;


        //} else { jumpTargetTextObject.active = false; }


        //TODO: in for testing reasons so I dont need to be near the computer to jump
        
        interactTimer += Time.deltaTime;
        if (interactTimer >= timeCost)
        {
            interactTimer = 0;
            CalculateJump();
        }
    }

    public void Interact(GameObject whoUsedMe)
    {
        interactTimer += Time.deltaTime;
        if (interactTimer >= timeCost) {
            interactTimer = 0;
            CalculateJump();
        }

    }

    //sets new space coordinate locations
    [PunRPC]
    public void SetAvailableJumpCoordinates(int newX,int newY,int newZ) {
        currentCoordinates = (newX * 100) + (newY * 10) + newZ;
        xCord = newX;
        yCord = newY;
        zCord = newZ;
        jumpTarget = null;
        oneStats.text = "Jump not calculated";

        twoStats.text = "Jump not calculated";

        threeStats.text = "Jump not calculated";
        fourStats.text = "Jump not calculated";

        fiveStats.text = "Jump not calculated";

        oneStats.text = "Jump not calculated";
        oneStats.transform.parent.gameObject.active = false;
        twoStats.transform.parent.gameObject.active = false;
        threeStats.transform.parent.gameObject.active = false;
        fourStats.transform.parent.gameObject.active = false;
        fiveStats.transform.parent.gameObject.active = false;
        sixStats.transform.parent.gameObject.active = false;

        //TODO: note plotted locations only appear after 1 jump is calculated. So when you jump in you have no immediatly available jump locations.
        //after you chart the first one some available jumps become visible at no extra cost
        jumpTargetOne.GetComponent<SpaceCoordinates>().SelectSpace(newX + 1, newY, newZ);
        jumpTargetTwo.GetComponent<SpaceCoordinates>().SelectSpace(newX - 1, newY, newZ);
        jumpTargetThree.GetComponent<SpaceCoordinates>().SelectSpace(newX, newY, newZ + 1);
        jumpTargetFour.GetComponent<SpaceCoordinates>().SelectSpace(newX, newY, newZ - 1);
        jumpTargetFive.GetComponent<SpaceCoordinates>().SelectSpace(newX, newY + 1, newZ);
        jumpTargetSix.GetComponent<SpaceCoordinates>().SelectSpace(newX, newY - 1, newZ);

    }

    //Map the available jump targets
    [PunRPC]
    public void SetFtlTarget() {
        if (jumpTargetOne.GetComponent<SpaceCoordinates>().mapped == true)
        {
            oneStats.transform.parent.gameObject.active = true;
            oneStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            
        }
         if (jumpTargetTwo.GetComponent<SpaceCoordinates>().mapped == true)
        {
            twoStats.transform.parent.gameObject.active = true;
            twoStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
        }
         if (jumpTargetThree.GetComponent<SpaceCoordinates>().mapped == true)
        {
            threeStats.transform.parent.gameObject.active = true;
            threeStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
        }
         if (jumpTargetFour.GetComponent<SpaceCoordinates>().mapped == true)
        {
            fourStats.transform.parent.gameObject.active = true;
            fourStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
        }
         if (jumpTargetFive.GetComponent<SpaceCoordinates>().mapped == true)
        {
            fiveStats.transform.parent.gameObject.active = true;
            fiveStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
        }
        
        if(jumpTargetSix.GetComponent<SpaceCoordinates>().mapped == true)
        {
            sixStats.transform.parent.gameObject.active = true;
            sixStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
        }

        if (jumpTargetOne.GetComponent<SpaceCoordinates>().mapped == false) {
            jumpTargetOne.GetComponent<SpaceCoordinates>().mapped = true; 
            oneStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            oneStats.transform.parent.gameObject.active = true;
        }
        else if (jumpTargetTwo.GetComponent<SpaceCoordinates>().mapped == false) { jumpTargetTwo.GetComponent<SpaceCoordinates>().mapped = true;
            twoStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            twoStats.transform.parent.gameObject.active = true;
        }
        else if (jumpTargetThree.GetComponent<SpaceCoordinates>().mapped == false) { jumpTargetThree.GetComponent<SpaceCoordinates>().mapped = true;
            threeStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            threeStats.transform.parent.gameObject.active = true;
        }
        else if (jumpTargetFour.GetComponent<SpaceCoordinates>().mapped == false) { jumpTargetFour.GetComponent<SpaceCoordinates>().mapped = true;
            fourStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            fourStats.transform.parent.gameObject.active = true;
        }
        else if (jumpTargetFive.GetComponent<SpaceCoordinates>().mapped == false) { jumpTargetFive.GetComponent<SpaceCoordinates>().mapped = true;
            fiveStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            fiveStats.transform.parent.gameObject.active = true;
        }
        else { jumpTargetSix.GetComponent<SpaceCoordinates>().mapped = true;
            sixStats.text = jumpTargetOne.GetComponent<SpaceCoordinates>().locationType;
            sixStats.transform.parent.gameObject.active = true;
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
        if (temp2.GetComponent<SpaceCoordinates>().mapped == true)
        {
            GetComponent<PhotonView>().RPC("SetJumpTargetForServer", PhotonTargets.AllViaServer,jumpTargetSelected);
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
    public void SetJumpTargetForServer(int thejumpTarget) {
        GameObject temp = JumpTargetRelativeCoordinates(thejumpTarget);
        jumpTarget = temp;
        jumpTargetCords = temp.GetComponent<SpaceCoordinates>().spaceLocation;
        

        
    }

    public void JumpTheFleet() {
        myShip.GetComponent<Galactica>().theFleet.GetComponent<PhotonView>().RPC("Jumping", PhotonTargets.AllViaServer, jumpTargetCords);
    
    }

    public void JumpThisShip()
    {
        if (jumpTarget != null)
        {

            numberOfJumps++;
            myShip.GetComponent<PhotonView>().RPC("StartJump", PhotonTargets.AllViaServer, jumpTargetCords);

           // myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
         
            xCord = jumpTarget.GetComponent<SpaceCoordinates>().thisX;
            yCord = jumpTarget.GetComponent<SpaceCoordinates>().thisY;
            zCord = jumpTarget.GetComponent<SpaceCoordinates>().thisZ;
            //SetAvailableJumpCoordinates(xCord,yCord,zCord);
            GetComponent<PhotonView>().RPC("SetAvailableJumpCoordinates", PhotonTargets.AllBufferedViaServer, xCord, yCord, zCord);

            //GetComponent<PhotonView>().RPC("SelectJumpDestinationOnServer", PhotonTargets.AllViaServer, jumpTargetCords);
        }
        else { Debug.Log("No Jump target Selected"); }
    }
    [PunRPC]
    public void SelectJumpDestinationOnServer(int coords)
    {
        //Not in use
        numberOfJumps++;
        myShip.GetComponent<PhotonView>().RPC("Jump", PhotonTargets.AllViaServer, coords);

       // myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
        myHangar.GetComponent<LandingBay>().Jumped();
        xCord = jumpTarget.GetComponent<SpaceCoordinates>().thisX;
        yCord = jumpTarget.GetComponent<SpaceCoordinates>().thisY;
        zCord = jumpTarget.GetComponent<SpaceCoordinates>().thisZ;
        //SetAvailableJumpCoordinates(xCord,yCord,zCord);
        GetComponent<PhotonView>().RPC("SetAvailableJumpCoordinates", PhotonTargets.AllBufferedViaServer, xCord, yCord, zCord);



    }

    public void CommandFleetToJump() { }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

             stream.SendNext(currentCoordinates);
            //  stream.SendNext(rotationObject.transform.rotation);
        }
        else
        {
             currentCoordinates = (int)stream.ReceiveNext();
            //jumpTarget  = (GameObject)stream.ReceiveNext();
        }
    }

}
