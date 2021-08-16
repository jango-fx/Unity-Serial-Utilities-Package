using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO.Ports;
using System.Threading;

/**

→ https://docs.microsoft.com/de-de/dotnet/api/system.io.ports?view=dotnet-plat-ext-5.0
→ https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ScriptableSingleton_1.html
→ https://baraujo.net/unity3d-making-singletons-from-scriptableobjects-automatically/
→ https://www.youtube.com/watch?v=6kWUGEQiMUI
→ https://www.youtube.com/watch?v=cH-QQoNNpaI

*/
namespace ƒx.UnityUtils.Serial
{
    [CreateAssetMenu(menuName = "Serial/SerialManager")]
    public class ScriptableSerialManager : ScriptableSingleton<ScriptableSerialManager>
    {
        Thread readThread = new Thread(Read);
        static SerialPort _serialPort = new SerialPort();
        static bool _continue;
        public string portName;

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Debug.Log(message);
                }
                catch (TimeoutException) { }
            }
        }

        public void Log()
        {
            Debug.Log("Serial Manager: " + JsonUtility.ToJson(this, true));
        }

    }

    static class SerialMenu
    {
        [MenuItem("Serial/Log")]
        static void LogMySingletonState()
        {
            ScriptableSerial.instance.Log();
        }
    }
}