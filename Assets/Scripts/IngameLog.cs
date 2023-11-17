using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class IngameLog : MonoBehaviour
{
    [System.Serializable]
    public struct LogMessage
    {
        public string content;
        public string stagTrace;
        public LogType type;
        //public Color color;
    }

    /*public enum MessageType
    {
        Invalid = -1,
        Normal,
        Error,
        Info,
        System,
        Player,
        UI,
        Other
     }*/

    #region SerializedFields

    [SerializeField] private List<LogMessage> messages = new List<LogMessage>();
    [SerializeField, Range(1,50)] private int maxMessages = 10;
    [SerializeField] private TextMeshProUGUI textField;
    //[SerializeField] private Color[] colors = new Color[Enum.GetValues(typeof(MessageType)).Length -1];

    #endregion

    #region PrivateFields

    private bool active = true;
    //private string color = "FFFFFF";
    /*private string prefix
    {
        get { return $"<color=#{color}>"; }
    }*/
    //private string suffix = "</color>";

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        if(textField != null) textField.text = "";
        Application.logMessageReceived += LogMessageReceived;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += LogMessageReceived;
    }

    #endregion

    #region MessageHandle

    private void LogMessageReceived(string content, string stackTrace, LogType type)
    {
        AddMessage(content, stackTrace, type);
    }

    public void AddMessage(string message, string stacktrace, LogType type)
    {
        string content = message;
        if (type == LogType.Error || type == LogType.Exception)
        {
            content = "<color=#FF0000>" + message + "</color>";
        }

        LogMessage newMessage = new LogMessage()
        {
            content = content,
            stagTrace = stacktrace,
            type = type,
        };
        if (messages.Count >= maxMessages)
        {
            DeleteOldestMessage();
        }
        messages.Add(newMessage);
        UpdateMessageView();
    }

    /*public void AddMessage(string message, MessageType type)
    {
        if (string.IsNullOrEmpty(message))
        {
            Debug.LogError("Cant Show Log Message !! -> Message is Empty");
            return;
        }

        color = ColorUtility.ToHtmlStringRGB(colors[(int)type]);
        LogMessage newMessage = new LogMessage()
        {
            content = prefix + message + suffix,
            //type = type,
        };
       
        if (messages.Count == maxMessages)
        {
            OldestFirstMesssage();
        }
        messages.Add(newMessage);
        UpdateMessageView();
        Debug.Log(newMessage.content);
    }*/

    public void ClearMessages()
    {
        messages.Clear();
    }

    private void DeleteOldestMessage()
    {
        if (messages.Count < 1) return;
        messages.RemoveAt(0);
    }

    #endregion

    #region View

    public void UpdateMessageView()
    {
        if (textField == null) return;

        string content = "";
        foreach (LogMessage message in messages)
        {
            content += message.content;
            content += "\n";
        }
        textField.text = content;
    }

    public void ChangeVisbilityState()
    {
        active = !active;
        textField.transform.parent.gameObject.SetActive(active);
    }

    #endregion
}



