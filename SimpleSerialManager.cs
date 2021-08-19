/*
References:
→ https://github.com/dwilches/Ardity
→ https://www.alanzucconi.com/2016/12/01/asynchronous-serial-communication/

→ https://docs.microsoft.com/de-de/dotnet/api/system.io.ports?view=dotnet-plat-ext-5.0
→ https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ScriptableSingleton_1.html
→ https://baraujo.net/unity3d-making-singletons-from-scriptableobjects-automatically/
→ https://www.youtube.com/watch?v=6kWUGEQiMUI
→ https://www.youtube.com/watch?v=cH-QQoNNpaI

*/


using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using ƒx.UnityUtils.Editor;
// using ƒx.UnityUtils.ScriptableObjects;


namespace ƒx.UnityUtils.Serial
{
    [CreateAssetMenu(menuName = "Serial/SimpleSerialManager")]
    public class SimpleSerialManager : ScriptableSingleton<SimpleSerialManager>
    {
        Thread thread = new Thread(SerialReadLoop);
        public static SerialPort port;
        [ReadOnly] public static bool isRunning;
        [HideInInspector] public string portName;
        [HideInInspector] public int baudRate = 9600;

        public delegate void SerialMessageHandler(string msg);
        public SerialMessageHandler serialMessageHandler;

        [HideInInspector] public SerialMessageEvent onMessageReceive; // TODO: make work with ScriptableObject-GameEvents?

        public void Start()
        {
            isRunning = true;
            if (port == null || !port.IsOpen)
                port = CreatePort();

            thread = new Thread(SerialReadLoop);
            thread.Start();
        }

        public void Stop()
        {
            HaltThread();
            thread.Abort();
            ClosePort();
        }

        public SerialPort CreatePort()
        {
            Debug.Log("[SERIAL MANAGER]: opening port");
            SerialPort _port = new SerialPort(portName, baudRate);
            _port.Parity = Parity.None;
            _port.StopBits = StopBits.One;
            _port.DataBits = 8;
            _port.Handshake = Handshake.None;//Handshake.RequestToSendXOnXOff;//Handshake.XOnXOff;//Handshake.RequestToSend;
            _port.DtrEnable = true;
            _port.RtsEnable = true;
            _port.ReadTimeout = 50;
            _port.Open();

            return _port;
        }

        public void ClosePort()
        {
            if (port != null)
            {
                port.Close();
                Debug.Log("[SERIAL MANAGER]: closing port");
            }
            else Debug.Log("[SERIAL MANAGER]: no port open");

        }

        public void HaltThread()
        {
            lock (this)
            {
                isRunning = false;
            }
        }

        public bool IsRunning()
        {
            lock (this)
            {
                return isRunning;
            }
        }

        static void SerialReadLoop()
        {
            while (SimpleSerialManager.instance.IsRunning())
            {
                try
                {
                    string message = port.ReadLine();
                    if (SimpleSerialManager.instance.serialMessageHandler != null)
                    {
                        SimpleSerialManager.instance.serialMessageHandler(message);
                        SimpleSerialManager.instance.onMessageReceive.Invoke(message);
                    }
                    Debug.Log("[SERIAL MANAGER]: received message\n" + message);
                }
                catch (TimeoutException) { }
            }
        }

        public void SendToSerial(string message)
        {
            port.WriteLine(message);
            port.BaseStream.Flush();
            Debug.Log("[SERIAL MANAGER]: sent message\n" + message);
        }

        // public void Log()
        // {
        //     Debug.Log("[SERIAL MANAGER]: " + JsonUtility.ToJson(this, true));
        // }

    }

    static class SimpleSerialManagerMenuItems
    {
        [MenuItem("Serial/Start")]
        static void StartSimpleSerialManager()
        {
            SimpleSerialManager.instance.Start();
        }

        [MenuItem("Serial/Stop")]
        static void StopSimpleSerialManager()
        {
            SimpleSerialManager.instance.Stop();
        }

        [MenuItem("Serial/Show SimpleSerialManager in Assets")]
        static void SelectSimpleSerialManager()
        {
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<SimpleSerialManager>(AssetDatabase.GetAssetPath(SimpleSerialManager.instance));
        }

        [MenuItem("Serial/Create SimpleSerialManager")]
        public static void CreateAsset()
        {
            ƒx.UnityUtils.ScriptableObjects.ScriptableObjectUtility.CreateAsset<SimpleSerialManager>("New SimpleSerialManager");
        }
    }

    [Serializable] public class SerialMessageEvent : UnityEvent<string> { }
}