using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSecondTutorialScript : MonoBehaviour {

    private bool loadSecond;

    private float startTime;

    private bool touched;

    void Start()
    {
        ControlObjectsHelper.RightController.GetComponent<ControllerTouchpadScript>().GripDown += Touched;
        ControlObjectsHelper.LeftController.GetComponent<ControllerTouchpadScript>().GripDown += Touched;
    }

    void Update()
    {
        Vector3 pos1 = ControlObjectsHelper.CameraEye.transform.position;
        pos1.y = 0.0f;

        Vector3 pos2 = gameObject.transform.position;
        pos2.y = 0.0f;


        if (!loadSecond && Vector3.Distance(pos1, pos2) < 1.8f && touched)
        {
            TutorialScript.CompletedLevel();
            loadSecond = true;
            startTime = Time.time;
        }

        if (loadSecond && Time.time - startTime > 3.0f)
        {
            TutorialScript.LoadSecondTutorial();
        }
    }

    private void Touched()
    {
        touched = true;
        ControlObjectsHelper.RightController.GetComponent<ControllerTouchpadScript>().GripDown -= Touched;
        ControlObjectsHelper.LeftController.GetComponent<ControllerTouchpadScript>().GripDown -= Touched;

    }


}
