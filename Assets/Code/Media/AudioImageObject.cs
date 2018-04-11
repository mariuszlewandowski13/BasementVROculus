#region Usings

using System.IO;

#endregion

public static class AudioImageObject {

    #region Private Properties

    private static string imgPath = "Assets/Artwork/";
    private static string imgName = "music.jpg";

    #endregion

    #region Methods

    public static byte [] GetAudioImageAsByteArray()
    {
        return File.ReadAllBytes(imgPath + imgName);
    }

    #endregion



}
