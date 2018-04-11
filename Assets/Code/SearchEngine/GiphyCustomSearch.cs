using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using SimpleJSON;
using System.Threading;

public class GiphyCustomSearch : MonoBehaviour
{

    public string query;
    public List<VideoObject> videos;

    public delegate void Result();
    public event Result resultReady;

    private WWW request;

    private int treshold = 25;


    public void Search(string text)
    {
        videos = new List<VideoObject>();
        query = text;
        StartCoroutine(SendRequest());
    }

    IEnumerator SendRequest()
    {
        query = query.Replace(" ", "+");
        WWW request = new WWW("http://api.giphy.com/v1/gifs/search?q=" + query + "&api_key=dc6zaTOxFJmzC");
        yield return request;
        if (request.error == null || request.error == "")
        {
           // File.WriteAllText("C:/Users/mariu/Desktop/parser.txt", request.text);
            var N = JSON.Parse(request.text);
            int count = N["pagination"]["count"].AsInt;

            for (int i = 0; i < (count >= treshold ? treshold : count); i++)
            {
                if (Int32.Parse(N["data"][i]["images"]["original"]["frames"]) > 24)
                {
                    string val = N["data"][i]["images"]["preview"]["mp4"];
                    string val2 = N["data"][i]["images"]["preview"]["width"];
                    string val3 = N["data"][i]["images"]["preview"]["height"];
                    //Debug.Log(i);
                    if (val != null && val2 != null && val3 != null)
                    {
                        fetch(val, val2, val3);
                    }
                } 
            }
        }
        else
        {
            Debug.Log("WWW error: " + request.error);
        }

        if (resultReady != null)
        {
            resultReady();
        }               
}

    public void fetch(string url, string width, string height)
    {
        videos.Add(new VideoObject( Int32.Parse(width), Int32.Parse(height), url));
    }

}
