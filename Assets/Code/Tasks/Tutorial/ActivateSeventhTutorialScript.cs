using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSeventhTutorialScript : MonoBehaviour {

    private bool loadFourth;
    private float startTime;

    private int counter = 4;

    private object counterLock = new object();

    void Update()
    {
        lock(counterLock)
        {
            if (!loadFourth && counter == 0)
            {

                TutorialScript.CompletedLevel();

                loadFourth = true;
                startTime = Time.time;
            }

            if (loadFourth && Time.time - startTime > 3.0f)
            {
                TutorialScript.LoadSeventhTutorial();
            }
        }
        
    }

    public void DecreaseCounter()
    {
        lock(counterLock)
        {
            counter--;
        }
    }

}
