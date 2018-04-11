using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTutorialScript : MonoBehaviour {

    private float startTime;
    private bool completed;

	void Update () {
        if (completed && (Time.time - startTime) > 20.0f)
        {
            TutorialScript.EndTutorial();
        }
	}

    public void SetTutorialEnded()
    {
        startTime = Time.time;
        completed = true;
    }

    
}
