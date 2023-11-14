using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class IngameLog : MonoBehaviour
{
    [SerializeField] private List<LogMessage> messages = new List<LogMessage>();
    [SerializeField] private int maxMessages = 10;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private Color[] colors = new Color[Enum.GetValues(typeof(MessageType)).Length -1];

    public static IngameLog instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    public void AddMessage(string message, MessageType type)
    {
        LogMessage newMessage = new LogMessage()
        {
            content = message,
            type = type,
            color = colors[(int)type],
        };

        if (messages.Count == maxMessages)
        {
            RemoveFirstMesssage();
        }
        messages.Add(newMessage);
        UpdateMessageView();

        string color = ColorUtility.ToHtmlStringRGB(newMessage.color);
        string prefix = $"<color=#{color}>";
        string suffix = "</color>";
        Debug.Log(prefix + message + suffix);
    }

    private void RemoveMessage(int index)
    {

    }

    private void RemoveFirstMesssage()
    {
        if (messages.Count < 1) return;
        messages.RemoveAt(0);
    }

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

}

public enum MessageType
{
    Invalid = -1,
    Normal,
    Error,
    Info,
    Player,
    UI,
    Other
}

[System.Serializable]
public struct LogMessage
{
    public string content;
    public MessageType type;
    public Color color;
}