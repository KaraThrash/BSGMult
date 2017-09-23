using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : Photon.PunBehaviour
{
    public bool canChangeScene;
    public bool destroyMe;
    public float dieClock;
    public bool tickClock;
    public bool saveNewObjects;
	// Use this for initialization
	void Start () {
        dieClock = 3;
        Debug.Log("start");
        if (saveNewObjects == true) {
            Debug.Log("Save them");

            ChangeScene[] saveThese = FindObjectsOfType(typeof(ChangeScene)) as ChangeScene[];
            Debug.Log(saveThese);
            foreach (ChangeScene go in saveThese) { go.GetComponent<ChangeScene>().tickClock = false; }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (tickClock == true) {
            dieClock -= Time.deltaTime;
            if (dieClock <= 0 && destroyMe == false) {
                Debug.Log("ran out of time");
                Destroy(this.gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) ) {
            GetComponent<PhotonView>().RPC("NewScene", PhotonTargets.AllViaServer);
            
        }
    }
    [PunRPC]
    public void NewScene()
    {
        dieClock = 3;
        tickClock = true;
        if (canChangeScene == true) { Application.LoadLevel("Game 1"); }
    }
    public void NowChangeScene()
    {
        Application.LoadLevel("Game 1");
    }

    void Awake()
    {
        // if (destroyMe == false) {
        tickClock = false;
        dieClock = 3;
        Debug.Log("Awake");
        DontDestroyOnLoad(transform.gameObject);
    //}
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
}
