using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TasksScript : MonoBehaviour {

    public delegate void TaskLevelEvent(TaskLevel taskLevel);
    public event TaskLevelEvent GetNewTaskLevel;

    public delegate void TaskEvent();
    public event TaskEvent UpdateTaskData;
    public event TaskEvent TaskCompleted;

    public bool fetchTasksLevel;

    private string message;
    private bool tasksLoaded;
    private bool userTasksToLoad;
    private bool userTasksLoaded;

    public bool levelComplited;
    private float completionTime;

    private TaskLevel actualTaskLevel;

    private List<int> tasksWaitingForApproval;

    void Start()
    {
        TasksManager.InitTasks();
        FetchTasksFromDb();
        fetchTasksLevel = true;
        tasksWaitingForApproval = new List<int>();
    }
    void Update()
    {
        if (tasksLoaded && !userTasksToLoad)
        {
            FetchUserTasksFromDb();
            userTasksToLoad = true;
        }

        if (userTasksLoaded && fetchTasksLevel)
        {
            FetchNewLevel();
            fetchTasksLevel = false;
            LoadWaitingTasks();
        }

        if (levelComplited && Time.time - completionTime > 5.0f)
        {
            FetchNewLevel();
            levelComplited = false;
        }
    }

    private void FetchTasksFromDb()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", ApplicationStaticData.userID);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetTasks.php", form);
        StartCoroutine(LoadTasks(w));
    }

    private void LoadWaitingTasks()
    {
        foreach (int taskNum in tasksWaitingForApproval)
        {
            UpdateTaskOccurance(taskNum);
        }
        tasksWaitingForApproval.Clear();
    }


    private void FetchNewLevel()
    {
        actualTaskLevel = TasksManager.GetActualTaskLevel();
        if (actualTaskLevel != null)
        {
            AddTasksEvents();
        }

        if (GetNewTaskLevel != null)
        {
            GetNewTaskLevel(actualTaskLevel);
        }

    }

    private void FetchUserTasksFromDb()
    {
        WWWForm form = new WWWForm();
        form.AddField("userID", ApplicationStaticData.userID);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetUserTasks.php", form);
        StartCoroutine(LoadUserTasks(w));
    }

    IEnumerator LoadTasks(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
      //  Debug.Log(message);
        string [] rows = message.Split(new string[] { "@@@@@" }, StringSplitOptions.None);
        foreach (string row in rows)
        {
            string [] fields = row.Split(new string[] { "#####" }, StringSplitOptions.None);
            if (fields.Length == 4)
            {
                TasksManager.AddTask(fields);
            }
        }
        tasksLoaded = true;
    }

    IEnumerator LoadUserTasks(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
      //  Debug.Log(message);
        string[] rows = message.Split(',');
        if (rows[0] == "null" || rows[0] == "")
        {
            string dbTasksEntry = TasksManager.GetTasksNewDbEntry(); 
            GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL("UPDATE USER_TASKS SET TASKS='" + dbTasksEntry + "' WHERE USER_STEAM_ID='" + ApplicationStaticData.userID+ "'");
        }
        else {
            TasksManager.LoadUserTasksCompletion(rows);
        }
        userTasksLoaded = true;
    }

    public void UpdateTaskOccurance(int taskNumber, string text = "")
    {
        if (!fetchTasksLevel)
        {
            if (CheckTaskConditions(taskNumber, text))
            {
                actualTaskLevel.tasks[taskNumber - 1].actualOccurances++;

                SaveUserProgress();

                if (actualTaskLevel.tasks[taskNumber - 1].actualOccurances >= actualTaskLevel.tasks[taskNumber - 1].targetOccurances)
                {
                    OnTaskCompleted(taskNumber);
                    if (TaskCompleted != null)
                    {
                        TaskCompleted();
                    }
                }
                if (UpdateTaskData != null)
                {
                    UpdateTaskData();
                }


                CheckTaskLevelCompletion();
            }
        }
        else {
            tasksWaitingForApproval.Add(taskNumber);
        }

        
        
    }

    private void OnTaskCompleted(int taskNumber)
    {
        actualTaskLevel.tasks[taskNumber - 1].completed = true;
        RemoveTaskEvent(taskNumber);
    }

    private void AddTasksEvents()
    {
        for (int i = 1; i <= TasksManager.tasksInLevel; ++i)
        {
            if (!actualTaskLevel.tasks[i - 1].completed)
            {
                switch (actualTaskLevel.levelNumber)
                {
                    case 1:
                        switch (i)
                        {
                            case 1:
                                ObjectSpawnerScript.taskNumber = 1;
                                break;
                            case 2:
                                ToolsButtons.taskNumber = 2;
                                break;
                            case 3:
                                LinesGL.taskNumber = 3;
                                break;
                        }
                        break;
                    case 2:
                        switch (i)
                        {
                            case 1:
                                CreateHallScript.taskNumber = 1;
                                break;
                            case 2:
                                LikeScript.taskNumber = 2;
                                break;
                            case 3:
                                OnLoadRoomLoadingScript.taskNumber = 3;
                                break;
                        }
                        break;
                    case 3:
                        switch (i)
                        {
                            case 1:
                                OnLoadRoomLoadingScript.taskNumber = 1;
                                break;
                            case 2:
                                ObjectSpawnerScript.taskNumber = 2;
                                break;
                            case 3:
                                PhotoSphereMiniatureWorldScript.taskNumber = 3;
                                PhotoSphereWorldScript.taskNumber = 3;
                                break;
                        }
                        break;
                }
            }
            
        } 
    }

    private void RemoveTaskEvent(int taskNumber)
    {
            switch (actualTaskLevel.levelNumber)
            {
                case 1:
                switch (taskNumber)
                {
                    case 1:
                        ObjectSpawnerScript.taskNumber = 0;
                        break;
                    case 2:
                        ToolsButtons.taskNumber = 0;
                        break;
                    case 3:
                        LinesGL.taskNumber = 0;
                        break;
                }
                break;
            case 2:
                switch (taskNumber)
                {
                    case 1:
                         CreateHallScript.taskNumber = 0;
                        break;
                    case 2:
                        LikeScript.taskNumber = 0;
                        break;
                    case 3:
                         OnLoadRoomLoadingScript.taskNumber = 0;
                        break;
                }
                break;
            case 3:
                switch (taskNumber)
                {
                    case 1:
                        OnLoadRoomLoadingScript.taskNumber = 0;
                        break;
                    case 2:
                        ObjectSpawnerScript.taskNumber = 0;
                        break;
                    case 3:
                        PhotoSphereMiniatureWorldScript.taskNumber = 0;
                        PhotoSphereWorldScript.taskNumber = 0;
                        break;
                }
                break;
        }
    }

    private void CheckTaskLevelCompletion()
    {
        if (actualTaskLevel.tasks[0].completed && actualTaskLevel.tasks[1].completed && actualTaskLevel.tasks[2].completed)
        {
            completionTime = Time.time;
            levelComplited = true; 
        }
    }

    private void SaveUserProgress()
    {
        string str = TasksManager.GetActualUserTaskStatusForDb();
        //Debug.Log(str);
        string sql = "UPDATE USER_TASKS SET TASKS = '" + str + "' WHERE USER_STEAM_ID = '" + ApplicationStaticData.userID +"'";
        GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL(sql);
    }

    private bool CheckTaskConditions(int taskNumber, string text)
    {
        switch (actualTaskLevel.levelNumber)
        {
            case 1:
                switch (taskNumber)
                {
                    case 1:
                        return ApplicationStaticData.roomToConnectName == ApplicationStaticData.userRoom && !TutorialScript.tutorialActive;
                    case 2:
                        return ApplicationStaticData.roomToConnectName == ApplicationStaticData.userRoom && (text.Trim() == "cat" || text.Trim() == "cats");
                    case 3:
                        return ApplicationStaticData.roomToConnectName == ApplicationStaticData.userRoom;
                }
                break;
            case 2:
                switch (taskNumber)
                {
                    case 1:
                        return ApplicationStaticData.roomToConnectName == ApplicationStaticData.worldRoomName;
                    case 2:
                        return true;
                    case 3:
                        return ((ApplicationStaticData.roomToConnectName != ApplicationStaticData.worldRoomName) && (ApplicationStaticData.roomToConnectName != ApplicationStaticData.userRoom));
                }
                break;
            case 3:
                switch (taskNumber)
                {
                    case 1:
                        return ApplicationStaticData.roomToConnectName == ApplicationStaticData.userRoom;
                    case 2:
                        return ((ObjectSpawnerScript.lastSpawned != null) && (ObjectSpawnerScript.lastSpawned.GetComponent<BrowserScript>() != null));
                    case 3:
                        return true;
                }
                break;
        }
        return true;
    }

}
