using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSixTutorialScript : MonoBehaviour {

    private GameObject collidingGameObj;
    private bool loadFourth;
    private float startTime;

    void Update()
    {
        if (!loadFourth && collidingGameObj != null && collidingGameObj.GetComponent<Shape3DScript>().shapeObject.shape3DObjectNumber == 5 && collidingGameObj.transform.parent == null)
        {

            TutorialScript.CompletedLevel();

            loadFourth = true;
            startTime = Time.time;
        }

        if (loadFourth && Time.time - startTime > 3.0f)
        {
            Destroy(collidingGameObj);
            TutorialScript.LoadSixTutorial();
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
