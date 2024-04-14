using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public string username;
    public int maxMessages = 30;

    public GameObject chatPanel, textObject;
    public TMP_InputField chatBox;

    [SerializeField]
    private List<Message> messageList = new List<Message>();

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (!string.IsNullOrEmpty(chatBox.text) && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.PlayerMessage);
            chatBox.text = "";
            chatBox.ActivateInputField();
        }
        else if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                chatBox.ActivateInputField();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("You pressed the space bar!", Message.MessageType.Info);
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (textObject == null || chatPanel == null)
        {
            Debug.LogError("Chat UI components not assigned in the inspector!");
            return;
        }

        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.RemoveAt(0);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        if (newText == null)
        {
            Debug.LogError("Failed to instantiate textObject!");
            return;
        }

        newMessage.textObject = newText.GetComponent<TMP_Text>();
        if (newMessage.textObject == null)
        {
            Debug.LogError("TMP_Text component not found on newText!");
            return;
        }

        newMessage.textObject.text = text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    private Color MessageTypeColor(Message.MessageType messageType)
    {
        switch (messageType)
        {
            case Message.MessageType.PlayerMessage:
                return Color.white;
            case Message.MessageType.Info:
                return new Color32(15, 98, 230, 255);
            default:
                return Color.black;
        }
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public TMP_Text textObject;
    public enum MessageType
    {
        PlayerMessage,
        Info,
    }
}