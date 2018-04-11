#region Usings

using UnityEngine;
using System;

#endregion

[Serializable]
public class VideoObject : MediaObject {


    #region Constructors

    public VideoObject(int width, int height, string videoFileName) : base(width, height, videoFileName)
    {

    }

    public VideoObject(string videoFileName) : base(1,1, videoFileName)
    {

    }

    public VideoObject(VideoObject vid) : base(vid.realWidth, vid.realHeight, vid.fileName)
    {
        this.SetSavedTransform(vid.GetSavedPosition(), vid.GetSavedRotation(), vid.GetSavedScale());
        this.actualRatio = vid.actualRatio;
        PhotonViewID = vid.PhotonViewID;
        saved = vid.saved;
    }

    public VideoObject(string[] row) : base(Int32.Parse(row[22]), Int32.Parse(row[23]), row[2]) //loading From SQL
    {
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        actualRatio = float.Parse(row[24]);
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }
    #endregion

    #region Methods

    public override string CreatSQLFromProperties()
    {
        return "null, 'VideoObject', '" + fileName + "', NULL, " + base.CreatSQLFromProperties() + ", null, null, null, null, null, null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 0" ;
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties();
    }

    #endregion
}
