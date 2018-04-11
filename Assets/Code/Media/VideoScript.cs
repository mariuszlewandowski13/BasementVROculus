#region Usings

using UnityEngine;

#endregion


public class VideoScript : MonoBehaviour, IResizable {

    #region Public Properties

    public VideoObject videoObject;

    #endregion


    #region Methods

    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "PhysicalObjectsSortingLayer";
    }

    public void SetVideoObject(VideoObject videoObj)
    {
        this.videoObject = new VideoObject(videoObj);
        gameObject.GetComponent<SceneObjectScript>().miniaturedSceneObject = videoObject;
        GetComponent<MediaPlayerCtrl>().m_strFileName = videoObject.fileName;
    }

    public void LoadFromVideoObject()
    {
        if (videoObject != null)
        {
            transform.position = videoObject.GetSavedPosition();
            transform.rotation = videoObject.GetSavedRotation();
            transform.localScale = videoObject.GetSavedScale();
        }
    }

    public void SetVideoObjectByNetwork(VideoObject videoObj)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("CreateAndSetVideoObject", PhotonTargets.Others, videoObj.realWidth, videoObj.realHeight, videoObj.fileName);
        }
    }

    [PunRPC]
    public void CreateAndSetVideoObject(int width, int height, string name)
    {
        SetVideoObject(new VideoObject(width, height, name));
    }

    public string GetResizableObjectPrefabName()
    {
        return "VideoObjectOnResize";
    }

    public void SetModyficableObject(ModyficableObject appearanceObject)
    {
        SetVideoObject((VideoObject)appearanceObject);
    }

    public ModyficableObject GetModyficableObject()
    {
        return videoObject;
    }


    public void SetMaterial(Material mat)
    {

    }

    public void UpdateActualRatio(float width, float height)
    {
        videoObject.UpdateActualRatio(width, height);
    }

    public void SetModyficableObjectByNetwork()
    {
        SetVideoObjectByNetwork(videoObject);
    }

    #endregion
}
