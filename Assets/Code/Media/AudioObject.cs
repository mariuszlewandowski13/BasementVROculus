#region Usings

using System;

#endregion


[Serializable]
public class AudioObject : MediaObject {


    #region Private Properties

    private static string audioPath = "music/"; 

    #endregion


    #region Constructors
    public AudioObject(int width, int height, string audioFileName) : base(width, height, audioFileName)
    {
    }

    public AudioObject(string audioFileName) : base(0,0, audioFileName)
    {
    }

    #endregion


    #region Methods

    public string GetAudioFileFullPath()
    {
        return audioPath + fileName;
    }

    #endregion
}
