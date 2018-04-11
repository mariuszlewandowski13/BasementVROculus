#region Usings

using UnityEngine;

#endregion

[RequireComponent(typeof(Renderer))]
public class AudioImageScript : MonoBehaviour {

    #region Methods
    void Start () {
        LoadImgAsMaterial();
    }

    private void LoadImgAsMaterial()
    {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(AudioImageObject.GetAudioImageAsByteArray());
        GetComponent<Renderer>().material.mainTexture = tex;
    }
    #endregion
}
