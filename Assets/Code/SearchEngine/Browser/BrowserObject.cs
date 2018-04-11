#region Usings

using UnityEngine;
using System;

#endregion

[Serializable]
public class BrowserObject : VisualObject {

    #region Public Properties
    public string url;

    #endregion

    #region Constructors

    public BrowserObject(string url) : base(1, 1)
    {
        this.url = url;
    }

    public BrowserObject(string url, int width, int height) : base(width, height)
    {
        this.url = url;
    }

    public BrowserObject(BrowserObject browser) : base(browser.realWidth, browser.realHeight)
    {
        this.url = browser.url;
        this.SetSavedTransform(browser.GetSavedPosition(), browser.GetSavedRotation(), browser.GetSavedScale());
        this.actualRatio = browser.actualRatio;
        PhotonViewID = browser.PhotonViewID;
        saved = browser.saved;
    }

    public BrowserObject(string[] row) : base(Int32.Parse(row[22]), Int32.Parse(row[23])) //loading From SQL
    {
        this.url = row[2];
        this.SetSavedTransform(new Vector3(float.Parse(row[4]), float.Parse(row[5]), float.Parse(row[6])), new Quaternion(float.Parse(row[7]), float.Parse(row[8]), float.Parse(row[9]), float.Parse(row[10])), new Vector3(float.Parse(row[11]), float.Parse(row[12]), float.Parse(row[13])));
        actualRatio = float.Parse(row[24]);
        PhotonViewID = Int32.Parse(row[25]);
        saved = true;
    }

    #endregion

    #region Methods

    public override string CreatSQLFromProperties()
    {
        return "null, 'BrowserObject', '" + url + "', 'null', " + base.CreatSQLFromProperties() + ", null, null, null, null, null, null, null, null, " + realWidth.ToString() + ", " + realHeight.ToString() + ", " + realRatio.ToString() + ", " + PhotonViewID.ToString() + ", 0";
    }

    public override string UpdateSQLProperties()
    {
        return base.UpdateSQLProperties() +  ", FILENAME= '" + url + "' ";
    }

    #endregion
}
