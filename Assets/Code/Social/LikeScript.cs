using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LikeScript : MonoBehaviour, IPointable {

    #region Public Properties

    public string roomName;
    public Transform likesCounter;
   
    public int counter;

    public static int taskNumber = 0;

    #endregion

    #region Private Properties

    private bool animationStarted;
    private float animationStartTime;
    private float animationDurationTime;
    private float lastUpdateTime;
    private float updateTime;
    private bool isMine;

    private bool highlighted = false;

    #endregion

    void Start()
    {
        animationDurationTime = 2.133f;
        updateTime = 5.0f;
        lastUpdateTime = Time.time;
        EndAnimation();
    }

   

    private void OnTriggerDown()
    {
        if (!animationStarted && !isMine)
        {
            SendLikeToDb();
            ChangeCounterLocally();
            StartAnimation();
            if (taskNumber > 0)
            {
                GameObject.Find("Player").GetComponent<TasksScript>().UpdateTaskOccurance(taskNumber);
            }
        }

    }

    private void StartAnimation()
    {
        animationStartTime = Time.time;
        transform.Find("like").GetComponent<Animator>().StopPlayback();
        animationStarted = true;
    }

    private void EndAnimation()
    {
        transform.Find("like").GetComponent<Animator>().StartPlayback();
        transform.Find("like").GetComponent<Animator>().Update(0.0f);
        animationStarted = false;
    }


    private void SendLikeToDb()
    {
        string sql = "UPDATE ROOMS SET LIKES = LIKES + 1 WHERE NAME = '" + roomName + "'";
        GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL(sql);
    }

    private void ChangeCounterLocally()
    {
        counter = Int32.Parse(likesCounter.GetComponent<TextMesh>().text);
        counter++;
        likesCounter.GetComponent<TextMesh>().text = counter.ToString();
    }

    void Update()
    {

        if (animationStarted && (Time.time - animationStartTime > animationDurationTime))
        {
            EndAnimation();
        }

        if (Time.time - lastUpdateTime > updateTime)
        {
            lastUpdateTime = Time.time;
            UpdateLikesFromDb();
        }

    }


    public void SetIsMine(bool isMine)
    {
        this.isMine = isMine;
        if (isMine)
        {
            transform.Find("like").gameObject.SetActive(false);
            transform.Find("Text008").gameObject.SetActive(false);
        }

    }

    private void UpdateLikesFromDb()
    {
        WWWForm form = new WWWForm();
        string sql = "SELECT LIKES FROM ROOMS WHERE NAME = '" + roomName + "'";
        form.AddField("sqlCommand", sql);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetLikes.php", form);
        StartCoroutine(request(w));
    }

    IEnumerator request(WWW w)
    {
        yield return w;
        string message;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
        //Debug.Log(message);
        int counter;
        if(Int32.TryParse(message, out counter))
        {
            UpdateLikesCounter(counter);
        }
       
    }

    private void UpdateLikesCounter(int i)
    {
        if (i > counter)
        {
            counter = i;
            likesCounter.GetComponent<TextMesh>().text = i.ToString();
        }
        
    }

   public void PointerIn()
    {
        if (!highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.toolsHighligthing);
            highlighted = true;
        }
    }

    public void PointerOut()
    {
        if (highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            highlighted = false;
        }
    }


    public void TriggerDown()
    {
        OnTriggerDown();
    }
}
