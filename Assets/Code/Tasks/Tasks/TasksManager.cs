using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct Task {

    public Task(int taskNum, string desc, int occur)
    {
        number = taskNum;
        description = desc;
        completed = false;
        targetOccurances = occur;
        actualOccurances = 0;
    }

    public string description;
    public int number;
    public int targetOccurances;
    public int actualOccurances;
    public bool completed;
}

public class TaskLevel {
    public TaskLevel(int number)
    {
        levelNumber = number;
        tasks = new Task[TasksManager.tasksInLevel];
    }
    public int levelNumber;
    public Task[] tasks;
}


public static class TasksManager {
    public static List<TaskLevel> tasksLevels;
    
    public static int tasksCount {
        get {
            return tasksLevels.Count * tasksInLevel;
        }
    }

    public static int tasksInLevel = 3;

    public static void AddTask(string[] task)
    {
        int taskLevel = Int32.Parse(task[0]);
        int taskNumber = Int32.Parse(task[1]);
        string taskDescr = task[2];
        int occurances = Int32.Parse(task[3]);

        if (tasksLevels.Count < taskLevel)
        {
            CreateNewTaskLevel(taskLevel);
        }

        Task newTask = new Task(taskNumber, taskDescr, occurances);
        tasksLevels[taskLevel - 1].tasks[taskNumber - 1] = newTask;
    }

    public static void LoadUserTasksCompletion(string[] tasks)
    {
            for (int i = 0; i < tasks.Length; i++)
            {
                int taskLevel = i / tasksInLevel;
                int taskNumber = i % tasksInLevel;
                tasksLevels[taskLevel].tasks[taskNumber].actualOccurances = Int32.Parse(tasks[i]);
                tasksLevels[taskLevel].tasks[taskNumber].completed = (tasksLevels[taskLevel].tasks[taskNumber].actualOccurances == tasksLevels[taskLevel].tasks[taskNumber].targetOccurances ? true : false);
            }
    }

    public static string GetTasksNewDbEntry()
    {
        string str = "";
        for (int i = 0; i < tasksCount; i++)
        {
            str += "0";
            if (i < tasksCount - 1) str += ",";
        }
        return str;
    }

    public static string GetActualUserTaskStatusForDb()
    {
        string str = "";
        int index = 0;
        foreach (TaskLevel taskLevel in tasksLevels)
        {
            for (int i = 0; i < taskLevel.tasks.Length; ++i)
            {
                str += taskLevel.tasks[i].actualOccurances;
                if (index < tasksCount - 1) str += ",";
                index++;
            }
        }
        return str;
    }

    public static TaskLevel GetActualTaskLevel()
    {
        TaskLevel taskLevelToReturn = null;
        foreach (TaskLevel taskLevel in tasksLevels)
        {
            for (int i = 0; i < tasksInLevel; i++)
            {
                if (taskLevel.tasks[i].completed == false)
                {
                    taskLevelToReturn = taskLevel;
                    break;
                }
            }
            if (taskLevelToReturn != null) break;
        }
        return taskLevelToReturn;
    }

    public static void InitTasks()
    {
        tasksLevels = new List<TaskLevel>();
    }

    private static void CreateNewTaskLevel(int newLevelNumber)
    {
        TaskLevel newTaskLevel = new TaskLevel(newLevelNumber);
        tasksLevels.Add(newTaskLevel);
    }

    

}
