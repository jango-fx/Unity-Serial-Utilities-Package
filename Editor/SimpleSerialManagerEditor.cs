using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using ƒx.UnityUtils.Serial;
using ƒx.UnityUtils;

namespace ƒx.UnityUtils.Serial
{
    [CustomEditor(typeof(SimpleSerialManager))]
    public class SimpleSerialManagerEditor : UnityEditor.Editor
    {
        SimpleSerialManager theScript;
        List<string> portsList = new List<string>();
        List<string> portNames = new List<string>();
        int portID;
        // Vector2 monitorScroll = new Vector2();
        // string monitorLog = "";

        void Awake()
        {
            theScript = (SimpleSerialManager)target;

            RefreshPortList();
            SetPort(theScript.portName);
            // SimpleSerialManager.instance.serialMessageHandler -= UpdateMonitor;
            // SimpleSerialManager.instance.serialMessageHandler += UpdateMonitor;
        }

        private void Update()
        {
            Repaint();
        }

        void SetPort(string portName)
        {
            if (portName != null)
            {
                int index = portsList.FindIndex(s => s.Contains(portName));
                if (index >= 0)
                    portID = index;
                else
                    Debug.Log("[SERIAL]: ›" + theScript.portName + "‹ not found");
            }
        }

        void OnDestroy() { theScript.portName = portsList[portID]; }

        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            if (GUILayout.Button("Refresh Ports List")) { RefreshPortList(); }

            GUIContent portSelection = new GUIContent("Select Serial Port: ");
            portID = EditorGUILayout.Popup(portSelection, portID, portNames.ToArray());
            theScript.portName = portsList[portID];
            GUIContent baudRateField = new GUIContent("Baud rate: ");
            EditorGUILayout.DelayedIntField(baudRateField, SimpleSerialManager.instance.baudRate);

            if (GUILayout.Button("Start Serial")) { SimpleSerialManager.instance.Start(); }
            if (GUILayout.Button("Stop Serial")) { SimpleSerialManager.instance.Stop(); }

            SerializedObject serObj = new SerializedObject(SimpleSerialManager.instance);
            SerializedProperty sprop = serializedObject.FindProperty("onMessageReceive");
            // EditorGUIUtility.LookLikeControls();
            EditorGUILayout.PropertyField(sprop);
            serializedObject.ApplyModifiedProperties();

            //? add SerialMonitor?
            // monitorScroll = EditorGUILayout.BeginScrollView(monitorScroll);
            // monitorLog = EditorGUILayout.TextArea(monitorLog, GUILayout.Height(100));
            // // EditorGUILayout.LabelField(monitorLog);
            // EditorGUILayout.EndScrollView();
            // if (GUILayout.Button("Clear Log")) { ClearMonitor(); }
            // if (GUILayout.Button("UpdateMonitor")) { UpdateMonitor("Lorem Ipsum"); }
        }

        // void UpdateMonitor(string msg)
        // {
        //     monitorLog += msg + "\n";
        // }

        // void ClearMonitor()
        // {
        //     monitorLog = "";
        // }

        void RefreshPortList()
        {
            portsList = new List<string>(System.IO.Ports.SerialPort.GetPortNames());
            portNames = new List<string>(portsList);

            for (int i = portNames.Count - 1; i >= 0; i--)
            {
                if (!portNames[i].StartsWith("/dev/tty."))
                {
                    portsList.RemoveAt(i);
                    portNames.RemoveAt(i);
                }
                else
                {
                    string[] split = portNames[i].Split('/');
                    split = split[split.Length - 1].Split('.');
                    portNames[i] = "[" + i + "]: " + split[split.Length - 1];
                }
            }

            Debug.Log(GetPortNames());
        }

        string GetPortNames()
        {
            string msg = "[SERIAL]: Found " + portsList.Count + " ports.";
            msg += portsList.ToArrayString();
 
            return msg;
        }
    }
}