using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareScript : MonoBehaviour {


    #region Private Properties

    private bool active = false;

    #endregion

    #region Methods

    void Start()
    {
        if (ApplicationStaticData.roomToConnectName == ApplicationStaticData.userRoom)
        {
            GetComponent<ObjectInteractionScript>().ControllerCollision += ControllerCollision;
        }
        else {
            gameObject.SetActive(false);
        }
        
    }

    private void ControllerCollision(GameObject gameObj, bool isEnter)
    {
        if (isEnter && !active)
        {
            active = true;
            gameObj.GetComponent<ControlObjects>().TriggerDown += OnTriggerDown;
        }
        else if (!isEnter && !GetComponent<ObjectInteractionScript>().GetIsSelected() && active)
        {
            active = false;
            gameObj.GetComponent<ControlObjects>().TriggerDown -= OnTriggerDown;
        }
    }

    private void OnTriggerDown(GameObject controller)
    {
        GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL("UPDATE ROOMS SET SHARED = 1, SHARED_DATE = now() WHERE NAME = '" + ApplicationStaticData.userRoom + "'");
    }

    void Update()
    {
        if (active && GetComponent<HighlightingSystem.Highlighter>() != null)
        {
            GetComponent<HighlightingSystem.Highlighter>().SeeThroughOff();
            GetComponent<HighlightingSystem.Highlighter>().On();
        }
    }

    #endregion
}
