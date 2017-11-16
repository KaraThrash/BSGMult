using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChatManager : Photon.PunBehaviour
{
    public string localPlayerName;
    public GameObject localPlayer;
    public InputField chatMsgField;
    public Text inputChatField;
    public Text chat1;
    public Text chat2;
    public Text chat3;
    public Text chat4;
    public Text chat5;
    public Text chat6;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendChat()
    {
        if (inputChatField.text != "")
        {
            GetComponent<PhotonView>().RPC("NewChat", PhotonTargets.AllViaServer, localPlayerName + " : " + inputChatField.text);
            inputChatField.transform.parent.gameObject.GetComponent<InputField>().text = "";
        }
    }
    [PunRPC]
    public void NewChat(string newChat)
    {
        chat1.text = chat2.text;
        chat2.text = chat3.text;
        chat3.text = chat4.text;
        chat4.text = chat5.text;
        chat5.text = chat6.text;
        chat6.text = newChat;


    }
}
