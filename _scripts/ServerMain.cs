using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ServerMain : Photon.PunBehaviour
{
    public GameObject galactica;
    public GameObject fleet;
    public GameObject baseStar;
    public GameObject jumpManager;
    public GameObject galacticaSpot;
    public GameObject basestarSpot;
    public List<GameObject> SelectableCharacters = new List<GameObject>();
    public List<GameObject> PlayersInGame = new List<GameObject>();
    // Use this for initialization
    void Start () {
        //PhotonNetwork.InstantiateSceneObject("Everything", Vector3.zero, new Quaternion(0, 0, 0, 0), 0, null) as GameObject;

        //galactica = PhotonNetwork.InstantiateSceneObject("Galactica", galacticaSpot.transform.position, new Quaternion(0, 0, 0, 0), 0, null) as GameObject;
        //jumpManager = galactica.transform.Find("JumpManager").gameObject;
        //jumpManager.transform.parent = null;
        //galactica.GetComponent<Galactica>().myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
        // baseStar = PhotonNetwork.InstantiateSceneObject("BaseStar", basestarSpot.transform.position, new Quaternion(0, 0, 0, 0), 0, null) as GameObject;
        
        //baseStar.GetComponent<BaseStar>().myHangar.GetComponent<PhotonView>().RPC("Jumped", PhotonTargets.AllViaServer);
        
       
    }
	
	// Update is called once per frame
	void Update () {
        if (photonView.isMine == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                galactica.GetComponent<PhotonView>().RPC("SetNewCords", PhotonTargets.AllViaServer, 3);
                
                SceneManager.LoadScene("Game 3");
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                galactica.GetComponent<PhotonView>().RPC("SetNewCords", PhotonTargets.AllViaServer, 4);
                SceneManager.LoadScene("Game 4");
            }
        }
    }
    void Awake()
    {
        // SceneManager.sceneLoaded()
        Debug.Log("ServerMain In New Scene");
       // DontDestroyOnLoad(this.gameObject);
    }

    // called first
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        
        }
        else
        {

        }
    }
}
