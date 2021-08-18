using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ƒx.UnityUtils.Serial;
using ƒx.UnityUtils.Editor;

public class SerialSendDemo : MonoBehaviour
{
    [ReadOnly] public SimpleSerialManager serialManager;
    public string msg = "";

    void Awake()
    {
        serialManager = SimpleSerialManager.instance;
    }

    [ContextMenu("Send Message")]
    void SendMessage()
    {
        SimpleSerialManager.instance.SendToSerial(msg);
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
