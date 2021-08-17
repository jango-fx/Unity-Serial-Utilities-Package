using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ƒx.UnityUtils.Serial;

public class SerialReceiveDemo : MonoBehaviour
{
    public SimpleSerialManager serialManager;
    public int messageCount = 0;
    public string lastMessage = "";

    void Start()
    {
        serialManager.serialMessageHandler += messageHandler;
    }

    void messageHandler(string msg)
    {
        messageCount++;
        lastMessage = msg;
    }

    [ContextMenu("Start Serial Manager")]
    void StartSerialManager()
    {
        serialManager.Start();
    }

    [ContextMenu("Stop Serial Manager")]
    void StopSerialManager()
    {
        serialManager.Stop();
    }
}
