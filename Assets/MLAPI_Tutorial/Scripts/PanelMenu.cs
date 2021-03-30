using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;

public class PanelMenu : MonoBehaviour
{
    public InputField inputFieldIp;
    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        gameObject.SetActive(false);
    }

    public void Join()
    {
        string ip = "";
        if (String.IsNullOrEmpty(inputFieldIp.text))
        {
            ip = "127.0.0.1";
        }
        else
        {
            ip = inputFieldIp.text;
        }

        NetworkManager.Singleton.GetComponent<UNetTransport>().ConnectAddress = ip;
        NetworkManager.Singleton.StartClient();
        gameObject.SetActive(false);
    }

    public void OnClickButtonHost()
    {
        Host();
    }

    public void OnClickButtonJoin()
    {
        Join();
    }
}
