using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecklistScript : MonoBehaviour {

    private TasksScript taskScript;
    private TaskLevel actualTaskLevel;

    private bool taskCompletedStars = false;
    private bool hidden = true;
    private bool visible = false;

    private bool toShow = false;
    private bool toHide = false;

    private float hideSpeed = 0.05f;

    private float maxScale = 1.0f;

    private Transform checklist;

    private float treshold = 30.0f;

    public ControlObjects myControlObject;

    public bool canShow = true;

    private bool showLevelCompleted;

    private AudioClip levelCompletedSound;
    private AudioClip taskCompletedSound;

    void Start () {
        taskScript = GameObject.Find("Player").GetComponent<TasksScript>();
        taskScript.GetNewTaskLevel += SetActualTaskInformations;
        taskScript.UpdateTaskData += UpdateTasksData;
        taskScript.TaskCompleted += OnTaskCompleted;
        taskScript.fetchTasksLevel = true;
        checklist = transform.Find("checklistPlane");
        levelCompletedSound = (AudioClip)Resources.Load("music/level");
        taskCompletedSound = (AudioClip)Resources.Load("music/quest");
    }

    void Update()
    {
        SetIfWatched();

        if (!visible && toShow && canShow)
        {
            Show();
        }

        if (!hidden && toHide)
        {
            Hide();
        }

        if (showLevelCompleted  != taskScript.levelComplited)
        {
            ChangeLevelComplitedVisualization();
        }
    }

    public  void ShowChecklistInfo(bool active)
    {
        transform.Find("checklistActive").gameObject.SetActive(active);
    }

    private void ChangeLevelComplitedVisualization()
    {
        showLevelCompleted = taskScript.levelComplited;
        transform.Find("completedLevel").gameObject.SetActive(showLevelCompleted);
        if (showLevelCompleted)
        {
            GetComponent<AudioSource>().PlayOneShot(levelCompletedSound, 1.0f);
        }
    }

    private void SetActualTaskInformations(TaskLevel newTask)
    {
        actualTaskLevel = newTask;
        if (newTask != null)
        {
            LoadTasksInfo();
        }
        else {
            LoadComplitedChallange();
        }
       
    }

    private void LoadTasksInfo()
    {
        if (actualTaskLevel != null)
        {
            LoadHeader();
            UpdateTasksData();
        }
    }

    private void LoadComplitedChallange()
    {
        checklist.Find("header").gameObject.SetActive(false);
        checklist.Find("task1").gameObject.SetActive(false);
        checklist.Find("task2").gameObject.SetActive(false);
        checklist.Find("task3").gameObject.SetActive(false);
        checklist.Find("completedTasks").gameObject.SetActive(true);
    }

    private void LoadHeader()
    {
        checklist.Find("header").Find("levelNumber").GetComponent<TextMesh>().text = actualTaskLevel.levelNumber.ToString();
    }

    private void UpdateTasksData()
    {
        LoadTask(1);
        LoadTask(2);
        LoadTask(3);
    }

    private void OnTaskCompleted()
    {
        if (!visible)
        {
            TaskCompletedVizualization(true);
        }
        
    }

    private void LoadTask(int taskNumber)
    {
        string text = actualTaskLevel.tasks[taskNumber - 1].description ;
        if (actualTaskLevel.tasks[taskNumber - 1].targetOccurances < 100)
        {
            text += " (" + actualTaskLevel.tasks[taskNumber - 1].actualOccurances + "/" + actualTaskLevel.tasks[taskNumber - 1].targetOccurances + ")";
        }
        checklist.Find("task" + taskNumber.ToString()).Find("taskText").GetComponent<TextMesh>().text = text;
        checklist.Find("task" + taskNumber.ToString()).Find("completed").gameObject.SetActive(actualTaskLevel.tasks[taskNumber - 1].completed);
    }

    private void TaskCompletedVizualization(bool active)
    {
        ChecklistEventVizualization(active);
        transform.Find("completedTask").gameObject.SetActive(active);
    }

    public void ChecklistEventVizualization(bool active)
    {
        if (active && myControlObject != null)
        {
            myControlObject.SetVibrations(2.0f);
            GetComponent<AudioSource>().PlayOneShot(taskCompletedSound, 1.0f);
        }
        transform.Find("arrows").gameObject.SetActive(active);
        taskCompletedStars = active;
        

    }



    private void Hide()
    {
        Vector3 scale = checklist.transform.localScale;
        scale.x -= hideSpeed;
        if (scale.x <= 0.0f)
        {
            scale.x = 0.0f;
            hidden = true;
            toHide = false;
        }
        visible = false;
        checklist.transform.localScale = scale;

       // Debug.Log("hide");
    }

    private void Show()
    {
        ShowChecklistInfo(false);
        Vector3 scale = checklist.transform.localScale;
        scale.x += hideSpeed;
        if (scale.x >= maxScale)
        {
            scale.x = maxScale;
            visible = true;
            toShow = false;

            if (taskCompletedStars)
            {
                TaskCompletedVizualization(false);
            }
        }

       

        hidden = false;

        checklist.transform.localScale = scale;

       // Debug.Log("show");
    }

    private void SetIfWatched()
    {
        bool open = CheckIfWatched();

        if (open && !visible && !toShow)
        {
            toShow = true;
            toHide = false;
        }
        else if (!open && !hidden && !toHide)
        {
            toHide = true;
            toShow = false;
        }
    }

    private bool CheckIfWatched()
    {
        Vector3 rotation = checklist.transform.rotation.eulerAngles;
       // Debug.Log("Rotation x: " + rotation.x + ", y: " + rotation.y + ", z: " + rotation.z);
        if (((rotation.x <= treshold) || (rotation.x >= 320 - treshold)) /*&& ((rotation.y >= 50 - treshold) && (rotation.y <= 50 + treshold))*/ && ((rotation.z <= treshold) || (rotation.z >= 360 - treshold)))
        {
            return true;
        }
        return false;
    }

    


}
