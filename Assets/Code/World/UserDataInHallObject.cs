using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserDataInHallObject : object {

    public string roomName;
    public string ownerName;
    public string scenName;
    public int objectsCount;
    public int views;
    public int usersInRoom;
    public string userImage;
    public int likes;

    public UserDataInHallObject(string[] row)
    {
        ownerName = row[0];
        roomName = row[1];
        scenName = row[2];
        objectsCount = Int32.Parse(row[3]);
        views = Int32.Parse(row[4]);
        usersInRoom = 0;
        userImage = row[7];
        likes = Int32.Parse(row[6]);
    }
}
