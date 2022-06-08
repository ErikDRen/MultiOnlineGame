using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChatManager : MonoBehaviour
{
    //public Player plMove;
    public PhotonView photonView;
    public GameObject BubbleSpeechObject;
    public Text UpdateText;

    private InputField ChatInputField;
    private bool DisableSend;

    public GameObject enable;
    private Player disable;

    private BaseEventData currentInputModule;


    private void Awake()
    {
        ChatInputField = GameObject.Find("ChatInputField").GetComponent<InputField>();
        disable = enable.GetComponent<Player>();

    }


    private void Update()
    {
        if(photonView.isMine)
        {
            if (!DisableSend && ChatInputField.isFocused)
            {
                disable.enabled = false;
                if(ChatInputField.text != "" && ChatInputField.text.Length > 0 && Input.GetKeyDown(KeyCode.RightShift))
                {
                    photonView.RPC("SendMessage", PhotonTargets.AllBuffered, ChatInputField.text);  
                    BubbleSpeechObject.SetActive(true);

                    ChatInputField.text = "";
                    DisableSend = true;
                    ChatInputField.OnDeselect(currentInputModule);


                }
            }
            
            
        }
        disable.enabled = true;
    }



    [PunRPC]
    private void SendMessage(string message)
    {
        UpdateText.text = message;
        StartCoroutine("Remove");
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(4f);
        BubbleSpeechObject.SetActive(false);
        DisableSend = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(BubbleSpeechObject.active); 
        }
        else if( stream.isReading)
        {
            BubbleSpeechObject.SetActive((bool)stream.ReceiveNext());
        }
    }
}
