using UnityEngine;
using System.Collections;

public class DemoRPGMovement : MonoBehaviour 
{
    public RPGCamera Camera;
    public  Vector3 position;
    public GameObject spawnLocation;
    void OnJoinedRoom()
    {
        Debug.Log("joined room");
        
        CreatePlayerObject();
    }

    void CreatePlayerObject()
    {
       // Vector3 position = new Vector3( 33.5f, 1.5f, 20.5f );

       // GameObject newPlayerObject = PhotonNetwork.Instantiate( "OldKyle", position, Quaternion.identity, 0 );
        //newPlayerObject.GetComponent<HumanControls>().SetAsMyPlayer();
         
        GameObject newPlayerObject = PhotonNetwork.Instantiate("OldKyle", Vector3.zero, Quaternion.identity, 0);
        Camera.Target = newPlayerObject.transform;
        //spawnLocation = GameObject.Find("GalacticaInterior");
        // position = spawnLocation.GetComponent<Galactica>().shipInterior.transform.position;
        // newPlayerObject.transform.position = spawnLocation.transform.position;

    }
}
