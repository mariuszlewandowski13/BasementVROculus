using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZenFulcrum.EmbeddedBrowser;
using System;

public class BrowserControllerInteractionScript : ClickMeshBrowserUI{

    private Browser browserComponent;

    private GameObject keyboard;
    private Vector3 lastPosition;
    public bool loadOnClick = false;

    public OVRInput.Controller controller;

    void Start()
    {
        lastPosition = new Vector3();
        browserComponent = GetComponent<Browser>();
        controller = OVRInput.Controller.RTouch;
    }

    public override void InputUpdate()
    {
        //Note: keyEvents gets filled in OnGUI as things happen. InputUpdate get called just before it's read.
        //To get the right events to the right place at the right time, swap the "double buffer" of key events.
        // Debug.Log("keyEvents: " + keyEvents.Count);

        if (ApplicationStaticData.CanInteract())
        {
            lock (keyEvents)
            {
                List<Event> tmp = keyEvents;
                keyEvents = keyEventsLast;
                keyEventsLast = tmp;
                keyEvents.Clear();
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller) && keyboard != null)
            {
                InputOff();
            }

            //Trace mouse from the main camera
            Ray mouseRayRight = ControlObjectsHelper.RightControllerRay;
            RaycastHit hit;
            Physics.Raycast(mouseRayRight, out hit, 1);

            //Debug.Log(hit.distance);

            if (hit.transform != meshCollider.transform)
            {
                //not looking at it.
                MousePosition = new Vector3(0, 0);
                MouseButtons = 0;
                MouseScroll = new Vector2(0, 0);

                MouseHasFocus = false;
                KeyboardHasFocus = true;

                LookOff2();

                //for drag and drop
                if (browserComponent.imgSrc != "")
                {
                    StartCoroutine(TryInstantiateImage(browserComponent.imgSrc));
                    ResetImageSource();
                }

                return;
            }

            if (!GetComponent<PhotonView>().isMine)
            {
                GetComponent<PhotonView>().RequestOwnership();
            }

            hitPoint = hit.point;
            if (browserComponent.IsLoaded)
            {
                browserComponent.EvalJS(BrowserHelperScript.getInputsFieldsTypes);
                browserComponent.EvalJS(BrowserHelperScript.setOnDragFunctionOnAHref);
                browserComponent.EvalJS(BrowserHelperScript.setOnDragFunctionOnImg);
            }

            LookOn2();
            MouseHasFocus = true;
            KeyboardHasFocus = true;

            MousePosition = hit.textureCoord;

            MouseButton buttons = (MouseButton)0;

            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller)) buttons |= MouseButton.Left;


            MouseButtons = buttons;
            lastPosition = hit.point;
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                ResetImageSource();
            }
        }
    }


    protected void CursorUpdated2()
    {
        Vector3[] points = new Vector3[] { ControlObjectsHelper.RightControlObject.transform.position , hitPoint };
        ControlObjectsHelper.RightControlObject.GetComponent<LineRenderer>().positionCount = 2;
        ControlObjectsHelper.RightControlObject.GetComponent<LineRenderer>().SetPositions(points);
    }

    protected void LookOn2()
    {
        if (BrowserCursor != null)
        {
            CursorUpdated2();
        }
        mouseWasOver = true;
    }

    protected void LookOff2()
    {
        if (BrowserCursor != null && mouseWasOver)
        {
            ControlObjectsHelper.RightControlObject.GetComponent<LineRenderer>().positionCount = 0;
        }
        mouseWasOver = false;
    }

    private void AddNewLetter(Event newLetter)
    {
        lock(keyEvents)
        {
            if (newLetter.character == '\n')
            {
                InputOff();
            }
            keyEvents.Add(newLetter);
        }
        
    }

    public void InputOn(string initialText)
    {
        if (keyboard == null)
        {
            GetComponent<PhotonView>().RequestOwnership();
            keyboard = KeyboardHandlerScript.InitializeKeyboard();
            keyboard.GetComponent<KeyboardScript>().ClearSearchBox();
            keyboard.GetComponent<KeyboardScript>().textBoxActive = true;
            keyboard.GetComponent<KeyboardScript>().text = initialText;
            keyboard.GetComponent<KeyboardScript>().NewKeyPressed += AddNewLetter;
        }
       
    }

    public void InputOff()
    {
        if (keyboard != null)
        {
            keyboard.GetComponent<KeyboardScript>().NewKeyPressed -= AddNewLetter;
            keyboard.GetComponent<KeyboardScript>().textBoxActive = true;
            KeyboardHandlerScript.CloseKeyBoard();
            keyboard = null;
        }
        
    }

    public void ResetImageSource()
    {
       browserComponent.imgSrc = "";
    }

    IEnumerator TryInstantiateImage(string name)
    {
        WWW www = new WWW(name);
        yield return www;

        if (www.texture != null && www.error == null)
        {
            try
            {
                ImageObject img = new ImageObject(www.texture.width, www.texture.height, name, "", LoadingType.remote);
                float ratio = (float)www.texture.height / (float)www.texture.width;
                GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnObjectFromWebBrowser(img, lastPosition, new Quaternion(), new Vector3(0.03f, 0.03f, 0.03f*ratio), GameObject.Find("Controller (right)").transform.Find("ControlObject").gameObject, true);
               
            }
            catch (Exception e)
            {
                Debug.Log(e);
                //throw;
            }
        }
    }

}
