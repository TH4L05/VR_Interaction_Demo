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

    private bool active;
    private string color = "FFFFFF";
    private string prefix
    {
        get { return $"<color=#{color}>"; }
    }
    private string suffix = "</color>";

    private void Awake()
    {
        if(textField != null) textField.text = "";
    }

    public void AddMessage(string message, MessageType type)
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
            type = type,
        };
       
        if (messages.Count == maxMessages)
        {
            RemoveFirstMesssage();
        }
        messages.Add(newMessage);
        UpdateMessageView();
        Debug.Log(newMessage.content);
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

    public void ChangeVisbilityState()
    {
        active = !active;
        textField.transform.parent.gameObject.SetActive(active);
    }
}

public enum MessageType
{
    Invalid = -1,
    Normal,
    Error,
    Info,
    System,
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