using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour {

    public delegate void BoxInteraction();
    public event BoxInteraction BoxEnter;
    public event BoxInteraction BoxExit;


    public Transform addedGameObject;
    private bool added = false;

    

    void OnTriggerEnter(Collider collider)
    {
        if (!added && collider.GetComponent<ImageScript>() != null)
        {
            addedGameObject = collider.transform;
            if (BoxEnter != null)
            {
                BoxEnter();
            }
            added = true;
            transform.Find("ok_tick").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (added && collider.GetComponent<ImageScript>() != null)
        {
            if (BoxExit != null)
            {
                BoxExit();
            }
            added = false;
            addedGameObject = null;
            transform.Find("ok_tick").gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (added && addedGameObject == null)
        {
            if (BoxExit != null)
            {
                BoxExit();
            }
            transform.Find("ok_tick").gameObject.SetActive(false);
            added = false;
        }
    }

}
