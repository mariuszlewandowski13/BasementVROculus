using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Steamworks;

public class CreateHallScript : MonoBehaviour {

    private string message;
    private bool top10loading = false;
    private bool lastLoggedLoading = false;
    private bool usersSelectedLoading = false;
    private bool dataLoaded = false;
    private int loaded = 0;
    private bool roomsListLoaded = false;


    private static RoomInfo[] roomsList;
    public static bool roomListAvailable;

    private object loadedLock = new object();


    private List<UserDataInHallObject> top10Users;
    private List<UserDataInHallObject> lastLoggedUsers;
    private List<UserDataInHallObject> usersSelectedByCreators;

    public static int taskNumber = 0;


    void Awake()
    {
        roomListAvailable = false;
    }

    // Use this for initialization
    void Start () {
        top10Users = new List<UserDataInHallObject>();
        lastLoggedUsers = new List<UserDataInHallObject>();
        usersSelectedByCreators = new List<UserDataInHallObject>();
        LoadUserBasement();
    }
	
	// Update is called once per frame
	void Update () {
        if (!top10loading)
        {
            LoadTop10();
            top10loading = true;
        }

        if (!lastLoggedLoading)
        {
            Load10LastLogged();
            lastLoggedLoading = true;
        }

        if (!usersSelectedLoading)
        {
            LoadUsersSelected();
            usersSelectedLoading = true;
        }

        //if (loaded >= 3  && roomListAvailable && !roomsListLoaded)
        //{
        //    LoadUserRoomsCounters();
        //    roomsListLoaded = true;
        //}


        if (loaded >= 3 && !dataLoaded /*&& roomsListLoaded*/)
        {
            LoadTop10DataIntoScene();
            LoadUserSelectedByCreatorsDataIntoScene();
            LoadUserLastLoggedDataIntoScene();

           
            dataLoaded = true;

            if (taskNumber > 0)
            {
                GetComponent<TasksScript>().UpdateTaskOccurance(taskNumber);
            }
        }

    }


    private void LoadTop10()
    {
        WWWForm form = new WWWForm();
        string sql = "SELECT a.USER_NAME, a.USER_ROOM, a.SCENE_NAME, a.ROOM_OBJECTS_COUNT, r.VIEWS_COUNT, a.USER_STEAM_ID, r.LIKES FROM APP_USERS a JOIN ROOMS r WHERE a.USER_ROOM = r.NAME AND SELECTED_FOR_MAIN_HALL >= 6  ORDER BY a.ROOM_OBJECTS_COUNT DESC";
        form.AddField("sqlCommand", sql);
        form.AddField("limit", 10);
        form.AddField("image", 1);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetDataForMainHall.php", form);
        StartCoroutine(loadUsersFromDb(w, top10Users));
    }

    IEnumerator loadUsersFromDb(WWW w, List<UserDataInHallObject> dest )
    {
        yield return w;
        if (w.error == null)
        {
            message = w.text;
        }
        else {
            message = "ERROR: " + w.error + "\n";
        }
        Debug.Log(message);
        string[] msg = null;
        string[] res = null;

        msg = message.Split(new string[] { "@@@@@" }, StringSplitOptions.None);
        foreach (string row in msg)
        {
           //Debug.Log(row);
            res = row.Split(new string[] { "#####" }, StringSplitOptions.None);
            if (res.Length > 1)
            {
                UserDataInHallObject newUser = new UserDataInHallObject(res);
                dest.Add(newUser);
            }
        }
        IncreaseLoaded();
        
    }

    private void Load10LastLogged()
    {
        WWWForm form = new WWWForm();
        string sql = "SELECT a.USER_NAME, a.USER_ROOM, a.SCENE_NAME, a.ROOM_OBJECTS_COUNT, r.VIEWS_COUNT, a.USER_STEAM_ID, r.LIKES FROM APP_USERS a JOIN ROOMS r WHERE a.USER_ROOM = r.NAME ORDER BY a.USER_LAST_LOGGED_DATE DESC";
        form.AddField("sqlCommand", sql);
        form.AddField("limit", 5);
        form.AddField("image", 1);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetDataForMainHall.php", form);
        StartCoroutine(loadUsersFromDb(w, lastLoggedUsers));
    }

    private void LoadUsersSelected()
    {
        WWWForm form = new WWWForm();
        string sql = "SELECT a.USER_NAME, a.USER_ROOM, a.SCENE_NAME, a.ROOM_OBJECTS_COUNT, r.VIEWS_COUNT, a.USER_STEAM_ID, r.LIKES FROM APP_USERS a JOIN ROOMS r WHERE a.USER_ROOM = r.NAME AND SELECTED_FOR_MAIN_HALL > 0 AND SELECTED_FOR_MAIN_HALL <= 5 ORDER BY SELECTED_FOR_MAIN_HALL ASC";
        form.AddField("sqlCommand", sql);
        form.AddField("limit", 5);
        form.AddField("image", 1);
        WWW w = new WWW(ApplicationStaticData.scriptsServerAdress + "GetDataForMainHall.php", form);
        StartCoroutine(loadUsersFromDb(w, usersSelectedByCreators));
    }


    private void IncreaseLoaded()
    {
        lock(loadedLock)
        {
            loaded++;
        }
    }

    private void LoadTop10DataIntoScene()
    {
        Transform ranks = GameObject.Find("common_room1").transform.Find("ranks");
        int index = 1;
        foreach (UserDataInHallObject user in top10Users)
        {
            LoadRank(ranks.Find("rank_" + index.ToString()).Find("rank_1"), user);
            index++;
        }
    }

    private void LoadUserSelectedByCreatorsDataIntoScene()
    {
        Transform ranks = GameObject.Find("common_room1").transform.Find("ranks2");
        int index = 1;
        foreach (UserDataInHallObject user in usersSelectedByCreators)
        {
            if (index >= 6)
            {
                break;
            }
            LoadRank(ranks.Find("rank_" + index.ToString()).Find("rank_1"), user);
            index++;
        }
    }

    private void LoadUserLastLoggedDataIntoScene()
    {
        Transform ranks = GameObject.Find("common_room1").transform.Find("ranks3");
        int index = 1;
        foreach (UserDataInHallObject user in lastLoggedUsers)
        {
            if (index >= 6)
            {
                break;
            }
            LoadRank(ranks.Find("rank_" + index.ToString()).Find("rank_1"), user);
            index++;
        }
    }

    private void LoadRank(Transform rank, UserDataInHallObject data)
    {
        rank.Find("Username").GetComponent<TextMesh>().text = data.ownerName;
        rank.Find("Views").GetComponent<TextMesh>().text = data.views.ToString();
        rank.Find("UsersInRoom").GetComponent<TextMesh>().text = data.usersInRoom.ToString();
        rank.Find("likesCount").GetComponent<TextMesh>().text = data.likes.ToString();
        rank.Find("like").GetComponent<LikeScript>().counter = data.likes;
        rank.Find("like").GetComponent<LikeScript>().roomName = data.roomName;
        rank.Find("like").GetComponent<LikeScript>().likesCounter = rank.Find("likesCount");
        rank.Find("like").GetComponent<LikeScript>().SetIsMine(ApplicationStaticData.userName == data.ownerName);


        rank.GetComponent<DoorOpenScript>().sceneName = data.scenName;
        rank.GetComponent<DoorOpenScript>().roomName = data.roomName;
        if (data.userImage != "null")
        {
            rank.Find("ImageFromRoom").GetComponent<ImageScript>().SetImageObject(new ImageObject(1,1, data.userImage, null, LoadingType.remote));
        }

        //getting avatar image
        string userID = data.roomName.Split(new string[] { "_" }, StringSplitOptions.None)[0];

        /*
         UNCOMMENT WHEn STEAMWORKS WILL BE AVAILABLE
         
         
         */

        //CSteamID steamID = new CSteamID(UInt64.Parse(userID));
        //int img = SteamFriends.GetMediumFriendAvatar(steamID);

        //uint height;
        //uint width;
        //SteamUtils.GetImageSize(img, out width, out height);

        //uint newSize = 4 * width * height * sizeof(char);
 
        //byte[] imgArray = new byte[newSize];
        //bool isImage = false; /*SteamUtils.GetImageRGBA(img, imgArray, (int)newSize);*/

        //if (isImage)
        //{
        //    Texture2D tex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
        //    tex.LoadRawTextureData(imgArray);
        //    tex.Apply();
        //    rank.Find("userImage").GetComponent<Renderer>().material.mainTexture = tex;
        //}
        //else {
            rank.Find("userImage").gameObject.SetActive(false);
      //  }

        

    }


    public static void UpdateUserCounters()
    {
        roomsList = PhotonNetwork.GetRoomList();
        roomListAvailable = true;
    }


    private void LoadUserRoomsCounters()
    {
        foreach (RoomInfo roomInfo in roomsList)
        {
            if (roomInfo.Name != ApplicationStaticData.worldRoomName && roomInfo.Name != "default2")
            {
                UserDataInHallObject item = top10Users.Find(x => x.roomName == roomInfo.Name);
                if (item != null)
                {
                    item.usersInRoom = roomInfo.PlayerCount;
                }

                item = usersSelectedByCreators.Find(x => x.roomName == roomInfo.Name);
                if (item != null)
                {
                    item.usersInRoom = roomInfo.PlayerCount;
                }

                item =lastLoggedUsers.Find(x => x.roomName == roomInfo.Name);
                if (item != null)
                {
                    item.usersInRoom = roomInfo.PlayerCount;
                }


            }

        }
    }

    private void LoadUserBasement()
    {
        Transform basementStairs = GameObject.Find("common_room1").transform.Find("stairs_basement");
        basementStairs.GetComponent<DoorOpenScript>().roomName = ApplicationStaticData.userRoom;
        basementStairs.GetComponent<DoorOpenScript>().sceneName = ApplicationStaticData.userScene;
    }

}
