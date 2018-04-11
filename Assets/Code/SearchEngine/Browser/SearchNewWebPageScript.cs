using UnityEngine;
using System.Collections;
using System;
using ZenFulcrum.EmbeddedBrowser;

public class SearchNewWebPageScript : MonoBehaviour {

    #region Private Properties

    private string text;
    private GameObject keyboard;
    private bool typing;
    private bool tryStartTyping;


    #endregion

    public OVRInput.Controller controller;

    public void StartSearch()
    {
        if (!typing && !KeyboardHandlerScript.keyboardActive)
        {
            GetComponent<PhotonView>().RequestOwnership();
            typing = true;
            keyboard = KeyboardHandlerScript.InitializeKeyboard();
            keyboard.GetComponent<KeyboardScript>().ClearSearchBox();
            keyboard.GetComponent<KeyboardScript>().textBoxActive = true;
            keyboard.GetComponent<KeyboardScript>().NewCharAdded += GetChar;
            text = "";
            tryStartTyping = false;
        }
        else {
            tryStartTyping = true;
        }
        
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) && typing)
        {
            EndTyping(true);
        }

        if (tryStartTyping)
        {
            StartSearch();
        }
    }

    private void GetChar(string newChar)
    {
        if (newChar == "back")
        {
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
            }
        }
        else if (newChar == "newLine")
        {
            text += Environment.NewLine;
        }
        else if (newChar == "done")
        {
            EndTyping();
        }
        else
        {
            text += newChar;
        }
    }

    private void EndTyping(bool typingBreak = false)
    {
        keyboard.GetComponent<KeyboardScript>().NewCharAdded -= GetChar;
        keyboard.GetComponent<KeyboardScript>().textBoxActive = false;
        KeyboardHandlerScript.CloseKeyBoard();
        if (!typingBreak)
        {
            GetComponent<BrowserScript>().SetNewUrl(text);
        }
        typing = false; 
    }
}
