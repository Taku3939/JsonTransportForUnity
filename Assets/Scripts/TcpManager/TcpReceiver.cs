using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TcpConsoleApp;
using UnityEngine;

public class TcpReceiver : MonoBehaviour
{
    private Server ser;
    // Start is called before the first frame update
    void Start()
    {
        Ser();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnApplicationQuit()
    {
        Server.Close();
    }

    private void Ser()
    {
        ser = new Server();
        //接続イベント
        ser.OnConnectEvent += (tcpClient) => {Debug.Log(tcpClient.Client.RemoteEndPoint); };
        //データ受信時イベント
        ser.receiveCallBack += (jsonBytes) =>
        {    //ここに処理を記入
            Debug.Log(Encoding.Default.GetString(jsonBytes));
        };
        //受信要求待ち
        ser.Accept();
    }
}
