using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxesBoardScript : MonoBehaviour {

    private int level = 0;
    private object levelLock = new object();

    private int[] tagNumbers;

    private bool completed = false;
    private float startTime;

    void Start()
    {
        tagNumbers = new int[5];

        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxScript>() != null)
            {
                child.GetComponent<BoxScript>().BoxEnter += IncreaseLevel;
                child.GetComponent<BoxScript>().BoxExit += DecreaseLevel;
            }  
        }
    }


    public void IncreaseLevel()
    {
       lock(levelLock)
        {
            level++;
            if (level > 0 && level <= 5)
            {
                transform.Find("loading").Find("Group" + level.ToString()).gameObject.SetActive(true); 
            }
            UpdateCounter();
        }
    }

    public void DecreaseLevel()
    {
        lock (levelLock)
        {
            if (level > 0)
            {
                transform.Find("loading").Find("Group" + level.ToString()).gameObject.SetActive(false);
            }
            level--;
            UpdateCounter();
        }
    }

    private void UpdateCounter()
    {
        transform.Find("loading").Find("text0").gameObject.SetActive(false);
        transform.Find("loading").Find("text1").gameObject.SetActive(false);
        transform.Find("loading").Find("text2").gameObject.SetActive(false);
        transform.Find("loading").Find("text3").gameObject.SetActive(false);
        transform.Find("loading").Find("text4").gameObject.SetActive(false);
        transform.Find("loading").Find("text5").gameObject.SetActive(false);

        transform.Find("loading").Find("text" + level.ToString()).gameObject.SetActive(true);
    }

    void Update()
    {
        if (level == 1 && TutorialScript.heightAdjust)
        {
            TutorialScript.TurnOffHeightAdjust();
        }
        if (level == 5 && !completed)
        {
            transform.Find("loading").gameObject.SetActive(false);
            SaveTagsNumbers();
            SendTagNumbersAndFinishTutorialToDb();


             TutorialScript.TutorialCompleted();

            completed = true;
            startTime = Time.time; 
        }
        if (completed && (Time.time - startTime) > 30.0f)
        {
            TutorialScript.EndTutorial();
        }
        
    }

    private void SaveTagsNumbers()
    {
        int index = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxScript>() != null)
            {
                tagNumbers[index] = Int32.Parse(child.GetComponent<BoxScript>().addedGameObject.GetComponent<ImageScript>().imageObject.imgName.Split('.')[0].Split('/')[1]);
                index++;
            }
        }
    }

    private void SendTagNumbersAndFinishTutorialToDb()
    {
        GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL("UPDATE APP_USERS SET TAG1 = '" + tagNumbers[0]+ "', TAG2 = '" + tagNumbers[1] + "', TAG3 = '" + tagNumbers[2] + "', TAG4 = '" + tagNumbers[3] + "', TAG5 = '" + tagNumbers[4] + "', TUTORIAL = 1 WHERE USER_STEAM_ID = " + ApplicationStaticData.userID);
    }


}
