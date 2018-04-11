#region Usings

using UnityEngine;

#endregion

public class LineDrawingScript : MonoBehaviour {

    #region Private Properties

    private LineDrawingObject marker;
    private GameObject controller;
    private int deactivatedController;

    private bool toActivate = false;
    private bool toDestroy = false;

    public OVRInput.Controller deviceController;

    #endregion

    #region Methods

    public void SetMarker(LineDrawingObject newLineDrawer, GameObject controller)
    {
        marker = newLineDrawer;
        this.controller = controller;
        TurnOn();
    }

    void Update()
    {
        if (toActivate)
        {
            ActiveTeleport();
            toActivate = false;
        }
        if (toDestroy)
        {
            Destroy(gameObject);
        }
    }

    public void TurnOn()
    {
        DeactiveTeleport();

        if (!TutorialScript.tutorialActive)
        {
            controller.transform.parent.GetComponent<ControllerTouchpadScript>().GripDown += BackButtonClicked;
        }
            
        if (GameObject.Find("Toolbar") != null)
        {
            GameObject.Find("Toolbar").transform.Find("buttons").Find("BackButton").GetComponent<BackButtonScript>().ButtonDown += BackButtonClicked;
        }
        

        GetComponent<LinesGL>().SetDrawLinesOn(transform.parent.Find("ControlObject").gameObject, marker);
        GetComponent<LinesGL>().SetLineColor(marker.color);
    }

    public void TurnOff()
    {
        if (!TutorialScript.tutorialActive)
        {
            controller.transform.parent.GetComponent<ControllerTouchpadScript>().GripDown -= BackButtonClicked;
        }

        toActivate = true;
        GetComponent<LinesGL>().SetDrawLinesOff(transform.parent.Find("ControlObject").gameObject);

        if (GameObject.Find("Toolbar") != null)
        {
            GameObject.Find("Toolbar").transform.Find("buttons").Find("BackButton").GetComponent<BackButtonScript>().ButtonDown -= BackButtonClicked;
        }
            
    }

    public void BackButtonClicked()
    {
        if (!GetComponent<LinesGL>().drawing || TutorialScript.tutorialActive)
        {
            TurnOff();
            toDestroy = true;
        }
    }

    private void ActiveTeleport()
    {
        GameObject.Find("Player").transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").GetComponent<TeleportVive>().canControllerTeleport[deactivatedController] = true;
    }

    private void DeactiveTeleport()
    {
        Transform cameraEye = GameObject.Find("Player").transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor");

        if (cameraEye.GetComponent<TeleportVive>().Controllers[0] == deviceController)
        {
            deactivatedController = 0;
            cameraEye.GetComponent<TeleportVive>().canControllerTeleport[0] = false;
        }
        else {
            deactivatedController = 1;

            cameraEye.GetComponent<TeleportVive>().canControllerTeleport[1] = false;
        }
    }

    #endregion
}
