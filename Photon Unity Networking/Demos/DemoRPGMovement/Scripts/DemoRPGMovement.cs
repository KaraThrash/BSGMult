using UnityEngine;
using System.Collections;

public class DemoRPGMovement : MonoBehaviour 
{
    public RPGCamera Camera;
    public  Vector3 position;
    public GameObject spawnLocation;
    public string playerObject;
    void OnJoinedRoom()
    {
        Debug.Log("joined room");
       // if (PhotonNetwork.countOfPlayers > 1) { CreatePlayerObject(); }
        CreatePlayerObject();
    }

    void CreatePlayerObject()
    {
       // Vector3 position = new Vector3( 33.5f, 1.5f, 20.5f );

       // GameObject newPlayerObject = PhotonNetwork.Instantiate( "OldKyle", position, Quaternion.identity, 0 );
        //newPlayerObject.GetComponent<HumanControls>().SetAsMyPlayer();
         
        GameObject newPlayerObject = PhotonNetwork.Instantiate(playerObject, new Vector3(-45.0f,0,(-40.0f * PhotonNetwork.room.playerCount)), Quaternion.identity, 0);
        //newPlayerObject.GetComponent<HumanControls>().SetAsMyPlayer();


       // Camera.Target = newPlayerObject.transform;
        //spawnLocation = GameObject.Find("GalacticaInterior");
        // position = spawnLocation.GetComponent<Galactica>().shipInterior.transform.position;
        // newPlayerObject.transform.position = spawnLocation.transform.position;

    }
}
