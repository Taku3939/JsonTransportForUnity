using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TcpConsoleApp;
using UnityEngine;

public class TcpSender : MonoBehaviour
{
    [SerializeField] GameObject _gameObject;
    private Client client;
    private TcpClient cl;
    // Start is called before the first frame update
    void Start()
    {
       
        Con(out client,out cl);
    }

    // Update is called once per frame
    void Update()
    {   
        //json
        String json = JsonUtility.ToJson(_gameObject.GetComponent<DataManager>());
        
        var ss = new SomeSend();
        //送信部
        ss.SendJ(client, cl, json);
    }

    private void OnApplicationQuit()
    {
        Client.Close();
    }

    private void Con(out Client client, out TcpClient cl)
    {
        //ip
        string ip = "localhost";

        //port
        int port = 30000;



        //接続開始
        client = new Client();
        client.Connect(out cl,ip, port);
        while ( cl == null)
        {
            client.Connect(out cl,ip, port);
        }
    }
}
