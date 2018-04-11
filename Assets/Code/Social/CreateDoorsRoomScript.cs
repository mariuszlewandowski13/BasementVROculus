using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class CreateDoorsRoomScript : MonoBehaviour {

    private string message;
    private int index = 1;

    void Awake()
    {
        LoadRooms();
    }


    private void LoadRooms()
    {
        WWWForm form = new WWWForm();
        string sql = "SELECT r.NAME, u.USER_NAME, u.SCENE_NAME, r.VIEWS_COUNT, u.TUTORIAL FROM ROOMS r JOIN APP_USERS u WHERE r.NAME = u.USER_ROOM AND r.SHARED_DATE IS NOT NULL ORDER BY r.SHARED_DATE DESC";
        form.AddField("sqlCommand", sql);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetRooms.php", form);
        StartCoroutine(loadRoomsFromDb(w));
    }

    IEnumerator loadRoomsFromDb(WWW w)
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }

        string[] msg = null;
        string[] res = null;

        msg = message.Split(new string[] { "@@@@@" }, StringSplitOptions.None);
        foreach (string row in msg)
        {
            res = row.Split(new string[] { "#####" }, StringSplitOptions.None);
            if (res.Length > 1)
            {
                LoadDataIntoDoor(res);
                index++;
            }
            if (index > 10)
            {
                break;
            }
        }
    }

    private void LoadDataIntoDoor(string [] doorInfo)
    {
       // GameObject.Find("")
    }
}
