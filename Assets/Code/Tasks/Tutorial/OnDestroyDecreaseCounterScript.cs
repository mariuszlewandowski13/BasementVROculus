using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyDecreaseCounterScript : MonoBehaviour {


    public GameObject objectWithCounter;


    void OnDestroy()
    {
        if (objectWithCounter != null)
        {
            objectWithCounter.GetComponent<ActivateSeventhTutorialScript>().DecreaseCounter();
        }
    }
}
