using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TutorialScript{

    public static bool tutorialActive;

    private static GameObject toolbar;

    private static MeshCollider teleportArea;

    private static GameObject navMesh;

    public static bool heightAdjust = true;

    private static GameObject sciana;

    private static GameObject tutorial;

    private static GameObject level;

    private static GameObject line;

    public static List<GameObject> lines = new List<GameObject>();

    private static int spawnCounter = 0;

    private static AudioClip taskCompletedSound;

    public static void StartTutorial()
    {
        taskCompletedSound = (AudioClip)Resources.Load("music/quest");
        GameObject.Find("Player").transform.Find("Controller (left)").Find("checklist").GetComponent<ChecklistScript>().canShow = false;
        GameObject.Find("Player").transform.Find("Controller (right)").Find("checklist").GetComponent<ChecklistScript>().canShow = false;

        toolbar = GameObject.Find("Player").transform.Find("Toolbar").gameObject;
        toolbar.SetActive(false);

        teleportArea = GameObject.Find("room").transform.Find("teleport").GetComponent<MeshCollider>();
        teleportArea.enabled = false;

        navMesh = GameObject.Find("Navmesh");
        navMesh.SetActive(false);

        GameObject.Find("Player").transform.position = new Vector3(-6.27f, 0.0f, 0.41f);

        tutorial = GameObject.Instantiate(GameObject.Find("Player").GetComponent<WorldObjects>().tutorial);

        sciana = tutorial.transform.Find("tutorialSciana").gameObject;

        LoadFirstTutorial();
    }


    public static void LoadFirstTutorial()
    {
        level = tutorial.transform.Find("level1").gameObject;
        level.SetActive(true);
        ControlObjectsHelper.CameraEye.GetComponent<HighlightingSystem.HighlightingRenderer>().iterations = 4;

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").gameObject.SetActive(true);
        ControlObjectsHelper.RightControlObject.GetComponent<ControlObjects>().SetVibrations(2.0f);

        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").gameObject.SetActive(true);
        ControlObjectsHelper.LeftControlObject.GetComponent<ControlObjects>().SetVibrations(2.0f);

        Vector3 rotationCamera = GameObject.Find("Player").transform.Find("Camera (eye)").rotation.eulerAngles;
        GameObject.Find("Player").transform.Rotate(0.0f, -rotationCamera.y, 0.0f);
    }

    public static void CompletedLevel()
    {
        tutorial.GetComponent<AudioSource>().PlayOneShot(taskCompletedSound, 1.0f);
        level.transform.Find("toHide").gameObject.SetActive(false);
        level.transform.Find("complete_move").gameObject.SetActive(true);
    }

    //public static void LoadSecondTutorial()
    //{
    //    SaveTutorialProgress(1);

    //    level.SetActive(false);
    //    level = tutorial.transform.FindChild("level2").gameObject;
    //    level.SetActive(true);

    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();


    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("trackpad_group002").FindChild("tutText").GetComponent<TextMesh>().text = "Use trigger to move object";
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("trackpad_group002").FindChild("tutText").GetComponent<TextMesh>().text = "Use trigger to move object";

    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);

    //}

    public static void LoadSecondTutorial()
    {
        SaveTutorialProgress(1);

        level.SetActive(false);
        level = tutorial.transform.Find("level2").gameObject;
        level.SetActive(true);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use touchpad to swipe";
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use touchpad to swipe";
        ControlObjectsHelper.RightControlObject.GetComponent<ControlObjects>().SetVibrations(2.0f);
        ControlObjectsHelper.LeftControlObject.GetComponent<ControlObjects>().SetVibrations(2.0f);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("arrows").gameObject.SetActive(true);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("arrows").gameObject.SetActive(true);

        toolbar.SetActive(true);

        toolbar.transform.Find("here_is_toolbar").gameObject.SetActive(true);

        ControllerTouchpadScript.TouchpadSwipe += SetTriggerTextOnDragAndDrop;

    }

    //public static void LoadThirdTutorial()
    //{
    //    SaveTutorialProgress(2);
    //    level.SetActive(false);
    //    level = tutorial.transform.FindChild("level3").gameObject;
    //    level.SetActive(true);

    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("r_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("l_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("r_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("l_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);


    //    ControlObjectsHelper.RightController.transform.FindChild("kontroler nowy").FindChild("trackpad_group002").FindChild("tutText").GetComponent<TextMesh>().text = "Use grip to duplicate object";
    //    ControlObjectsHelper.LeftController.transform.FindChild("kontroler nowy").FindChild("trackpad_group002").FindChild("tutText").GetComponent<TextMesh>().text = "Use grip to duplicate object";

    //}

    public static void LoadThirdTutorial()
    {
        SaveTutorialProgress(2);
        level.SetActive(false);
        level = tutorial.transform.Find("level3").gameObject;
        level.SetActive(true);

        tutorial.GetComponent<AudioSource>().PlayOneShot(taskCompletedSound, 1.0f);

        toolbar.GetComponent<ToolbarManagerScript>().LoadTutorialShapes3D();

        toolbar.transform.Find("infoBox").gameObject.SetActive(true);
        toolbar.transform.Find("infoBox").GetComponent<TextMesh>().text = "Drag & drop 3 objects (0 / 3)";
        toolbar.transform.Find("here_is_toolbar").gameObject.SetActive(false);


        ObjectSpawnerScript.ObjectSpawned += CountSpawnedObjects;

    }

    public static void LoadFourthTutorial()
    {
        SaveTutorialProgress(3);
        level.SetActive(false);
        level = tutorial.transform.Find("level4").gameObject;
        level.SetActive(true);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use trigger to draw";
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use trigger to draw";

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("r_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("l_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("r_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("l_gripper_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

        line = GameObject.Find("Player").GetComponent<ObjectSpawnerScript>().SpawnObject(new LineDrawingObject(HelperFunctionsScript.GetRandomColor(), 5), new Vector3(), new Quaternion(), new Vector3(), ControlObjectsHelper.RightControlObject);
        level.transform.Find("GameObject").GetComponent<ActivateFifthTutorialScript>().line = line;
    }

    public static void LoadFifthutorial()
    {
        SaveTutorialProgress(4);
        line.GetComponent<LineDrawingScript>().BackButtonClicked();

        level.SetActive(false);
        level = tutorial.transform.Find("level5").gameObject;
        level.SetActive(true);

        foreach (GameObject line in lines)
        {
            GameObject.Destroy(line);
        }

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use touchpad to swipe";
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use touchpad to swipe";

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("arrows").gameObject.SetActive(true);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("arrows").gameObject.SetActive(true);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

        toolbar.SetActive(true);
        toolbar.GetComponent<ToolbarManagerScript>().LoadTutorialShapes3D();

        toolbar.transform.Find("here_is_toolbar").gameObject.SetActive(true);

        ControllerTouchpadScript.TouchpadSwipe += SetTriggerTextOnDragAndDrop;
    }

    public static void LoadSixTutorial()
    {
        toolbar.transform.Find("TileRotationCenter3").Find("ToolbarTile").GetComponent<ToolbarTileMiniatureScript>().number = 2;
        SaveTutorialProgress(5);
        level.SetActive(false);
        level = tutorial.transform.Find("level6").gameObject;
        level.SetActive(true);

        toolbar.transform.Find("here_is_toolbar").gameObject.SetActive(false);

        toolbar.SetActive(false);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use trigger to select and throw objects";
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use trigger to select and throw objects";

        
    }

    public static void LoadSeventhTutorial()
    {
        SaveTutorialProgress(6);
        level.SetActive(false);
        level = tutorial.transform.Find("level7").gameObject;
        level.SetActive(true);


        toolbar.SetActive(true);
      //  toolbar.GetComponent<ToolbarManagerScript>().LoadTutorialTags();

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").gameObject.SetActive(false);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").gameObject.SetActive(false);

    }

    public static void SetTriggerTextOnDragAndDrop(Vector2 axis, bool first)
    {
        ControllerTouchpadScript.TouchpadSwipe -= SetTriggerTextOnDragAndDrop;

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use trigger to drag & drop";
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").GetComponent<TextMesh>().text = "Use trigger to drag & drop";

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.red, Color.white, 1.0f);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("arrows").gameObject.SetActive(false);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("arrows").gameObject.SetActive(false);

        //new tutorial
        LoadThirdTutorial();

    }

    public static void TurnOffHeightAdjust()
    {
        toolbar.transform.Find("buttons").Find("PositionButton").Find("tutText").gameObject.SetActive(false);
        toolbar.transform.Find("buttons").Find("PositionButton").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        heightAdjust = false;
    }

    //public static void TutorialCompleted()
    //{
    //    SaveTutorialProgress(7);
    //    SaveTutorialCompleted();

    //    CompletedLevel();

    //    ControlObjectsHelper.CameraEye.GetComponent<HighlightingSystem.HighlightingRenderer>().iterations = 2;

    //    toolbar.GetComponent<ToolbarManagerScript>().MainBar();

    //    TutorialScript.tutorialActive = false;

    //    GameObject.Destroy(sciana);
    //}

    public static void TutorialCompleted()
    {
        SaveTutorialProgress(3);
        SaveTutorialCompleted();

        CompletedLevel();

        //new tutorial
        level.transform.Find("complete_move").GetComponent<EndTutorialScript>().SetTutorialEnded();

        ControlObjectsHelper.CameraEye.GetComponent<HighlightingSystem.HighlightingRenderer>().iterations = 2;

        toolbar.GetComponent<ToolbarManagerScript>().MainBar();

        teleportArea.enabled = true;

        navMesh.SetActive(true);

        toolbar.transform.Find("infoBox").gameObject.SetActive(false);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").gameObject.SetActive(false);
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").Find("tutText").gameObject.SetActive(false);

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trigger_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

        TutorialScript.tutorialActive = false;

        //tutorial.transform.FindChild("level4").gameObject.SetActive(true);

        GameObject.Find("Player").transform.Find("Controller (left)").Find("checklist").GetComponent<ChecklistScript>().canShow = true;
        GameObject.Find("Player").transform.Find("Controller (right)").Find("checklist").GetComponent<ChecklistScript>().canShow = true;
        GameObject.Find("Player").transform.Find("Controller (right)").Find("checklist").GetComponent<ChecklistScript>().ShowChecklistInfo(true);
        GameObject.Find("Player").transform.Find("Controller (left)").Find("checklist").GetComponent<ChecklistScript>().ShowChecklistInfo(true);

        GameObject.Find("Player").transform.Find("Controller (left)").Find("checklist").GetComponent<ChecklistScript>().ChecklistEventVizualization(true);
        GameObject.Find("Player").transform.Find("Controller (right)").Find("checklist").GetComponent<ChecklistScript>().ChecklistEventVizualization(true);


        GameObject.Destroy(sciana);



    }

    public static void EndTutorial()
    {
        level.SetActive(false);
        //toolbar.transform.FindChild("buttons").FindChild("BackButton").gameObject.SetActive(true);
    }

    private static void CountSpawnedObjects()
    {
        spawnCounter++;
        toolbar.transform.Find("infoBox").GetComponent<TextMesh>().text = "Drag & drop 3 objects ("+ spawnCounter.ToString() +" / 3)";
        if (spawnCounter >= 3){
            ObjectSpawnerScript.ObjectSpawned -= CountSpawnedObjects;
            TutorialCompleted();
        }
    }

    private static void UnloadFirtsTutorial()
    {
        ControlObjectsHelper.CameraEye.GetComponent<HighlightingSystem.HighlightingRenderer>().iterations = 2;

        ControlObjectsHelper.RightController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();

        ControlObjectsHelper.LeftController.transform.Find("kontroler nowy").Find("trackpad_group002").GetComponent<HighlightingSystem.Highlighter>().FlashingOff();
    }

    private static void SaveTutorialProgress(int level)
    {
        GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL("UPDATE TUTORIAL_3 SET LEVEL" + level.ToString()+" = 1 WHERE USER_STEAM_ID = '" + ApplicationStaticData.userID + "'");
    }

    private static void SaveTutorialCompleted()
    {
        GameObject.Find("Player").GetComponent<DatabaseHandler>().ExequteSQL("UPDATE APP_USERS SET TUTORIAL = 1 WHERE USER_STEAM_ID = '" + ApplicationStaticData.userID + "'");
    }
}
