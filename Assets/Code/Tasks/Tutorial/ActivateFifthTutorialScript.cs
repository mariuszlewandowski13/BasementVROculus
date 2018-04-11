using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFifthTutorialScript : MonoBehaviour {

    public GameObject line;
    private bool complitedFourth;

    private float time;

    void Update()
    {
        if (line != null && !complitedFourth)
        {
            if (line.GetComponent<LinesGL>().pointsCounter > 40)
            {
                TutorialScript.CompletedLevel();
                complitedFourth = true;
                time = Time.time;
            }
        }

        if (complitedFourth && Time.time - time > 3.0f)
        {
            TutorialScript.LoadFifthutorial();
        }
    }
}
