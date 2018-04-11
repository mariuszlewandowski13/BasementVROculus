using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateThirdTutorialScript : MonoBehaviour {

    private GameObject collidingGameObj;
    private bool loadThird;
    private float startTime;

    void Update()
    {
        if (!loadThird && collidingGameObj != null && !collidingGameObj.GetComponent<ImageMoveScript>().active)
        {
            TutorialScript.CompletedLevel();
            loadThird = true;
            startTime = Time.time;
        }

        if (loadThird && Time.time - startTime > 3.0f)
        {
            TutorialScript.LoadThirdTutorial();
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.GetComponent<ImageMoveScript>() != null)
        {
            collidingGameObj = collider.gameObject;
        }
    }


    void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject == collidingGameObj)
        {
            collidingGameObj = null;
        }
    }
}
