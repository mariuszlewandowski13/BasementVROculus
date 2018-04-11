#region Usings

using System;
using UnityEngine;

#endregion

[Serializable]
public abstract class MediaObject : ModyficableObject {

    #region Public Properties

    public string fileName;

    #endregion

    #region Constructors
    protected MediaObject(int width, int height, string fileName) : base(width, height)
    {
        this.fileName = fileName; 
    }

    #endregion


}
