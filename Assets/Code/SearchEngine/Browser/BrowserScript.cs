using UnityEngine;
using System.Collections;
using ZenFulcrum.EmbeddedBrowser;


[RequireComponent(typeof(Browser))]
[RequireComponent(typeof(Renderer))]
public class BrowserScript : MonoBehaviour, IResizable {

    #region Public Properties

    public BrowserObject browserObject;

    #endregion

    #region Methods

    void Start()
    {
        gameObject.GetComponent<Renderer>().sortingLayerName = "PhysicalObjectsSortingLayer";
    }

    //void Update()
    //{
    //    if (pageReadyToLoad && GetComponent<Browser>().IsReady)
    //    {
    //        GetComponent<Browser>().Url = browserObject.url;
    //        pageReadyToLoad = false;
    //    }
    //}


    private void SetScaleRatio()
    {
        Vector3 scale = browserObject.GetSavedScale();
        if (scale != new Vector3())
        {
            gameObject.transform.localScale = scale;
        }
        else
        {
            float heightScale = transform.localScale.z;
            heightScale *= browserObject.realRatio;
            heightScale -= transform.localScale.z;
            transform.localScale += new Vector3(heightScale, 0.0f, 0.0f);
        }
    }


    public void SetBrowserObject(BrowserObject browser)
    {
        this.browserObject = new BrowserObject(browser);
        GetComponent<Browser>().Url = browser.url;
        
        //GetComponent<Browser>().LoadURL(browser.url, true);
    }

    public void SetNewUrl(string url)
    {
        //GetComponent<Browser>().Url = url;
        GetComponent<Browser>().LoadURL(url, true);
        //browserObject.url = url;
       // SetUrlByNetwork(url);
    }

    public void LoadFromImageObject()
    {
        if (browserObject != null)
        {
            transform.position = browserObject.GetSavedPosition();
            transform.rotation = browserObject.GetSavedRotation();
            transform.localScale = browserObject.GetSavedScale();
        }
    }


    public void SetBrowserObjectByNetwork(BrowserObject browser)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("CreateAndSetBrowserObject", PhotonTargets.Others, browser.realWidth, browser.realHeight, browser.url);
        }
    }

    [PunRPC]
    public void CreateAndSetBrowserObject(int width, int height, string url)
    {
        SetBrowserObject(new BrowserObject(url, width, height));
    }

    public void SetUrlByNetwork(string url)
    {
        if (PhotonNetwork.inRoom)
        {
            GetComponent<PhotonView>().RPC("SetUrlFromNetwork", PhotonTargets.Others, url);
        }
    }

    [PunRPC]
    public void SetUrlFromNetwork(string url)
    {
        if (url != this.browserObject.url)
        {
            GetComponent<Browser>().LoadURL(url, true);
            //browserObject.url = url;
        }
        
    }

    public string GetResizableObjectPrefabName()
    {
        return "BrowserOnResize";
    }

    public void SetModyficableObject(ModyficableObject appearanceObject)
    {
    }

    public ModyficableObject GetModyficableObject()
    {
        return browserObject;
    }


    public void SetMaterial(Material mat)
    {

    }

    public void UpdateActualRatio(float width, float height)
    {
        browserObject.UpdateActualRatio(width, height);
    }

    public void SetModyficableObjectByNetwork()
    {
    }


    #endregion

}
