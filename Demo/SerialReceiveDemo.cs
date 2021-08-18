using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ƒx.UnityUtils.Serial;
using ƒx.UnityUtils.Editor;

public class SerialReceiveDemo : MonoBehaviour
{
    [ReadOnly] public SimpleSerialManager serialManager;
    [ReadOnly] public int messageCount = 0;
    [ReadOnly] public string lastMessage = "";

    void Reset()
    {
        SimpleSerialManager.instance.serialMessageHandler -= messageHandler;
        SimpleSerialManager.instance.serialMessageHandler += messageHandler;
    }

    void Awake()
    {
        serialManager = SimpleSerialManager.instance;
        SimpleSerialManager.instance.serialMessageHandler -= messageHandler;
        SimpleSerialManager.instance.serialMessageHandler += messageHandler;
        SimpleSerialManager.instance.Start();
    }

    void OnDestroy()
    {
        SimpleSerialManager.instance.Stop();
    }

    void messageHandler(string msg)
    {
        messageCount++;
        lastMessage = msg;
    }

    [ContextMenu("Start Serial Manager")]
    void StartSerialManager()
    {
        SimpleSerialManager.instance.Start();
    }

    [ContextMenu("Stop Serial Manager")]
    void StopSerialManager()
    {
        SimpleSerialManager.instance.Stop();
    }
}
