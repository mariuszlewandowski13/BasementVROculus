using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePositionScript : MonoBehaviour {

    private Color prevColor;
    private Vector3 startPosition;

    public Vector3 teleportPosition;

    private bool isUp;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    public void highlightRed()
    {
        if (GetComponent<Renderer>() != null)
        {
            prevColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.SetColor("_Color", ApplicationStaticData.doorsHighligthing);

        }
        ChangeChildrenColor(ApplicationStaticData.doorsHighligthing);
    }

    public void disableHighlight()
    {
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.SetColor("_Color", prevColor);

        }
        RestoreChildrenColor();
    }

    private void ChangeChildrenColor(Color col)
    {
        ChangeColorScript[] children = transform.GetComponentsInChildren<ChangeColorScript>();
        foreach (ChangeColorScript child in children)
        {
            child.ChangeColor(col);
        }
    }

    private void RestoreChildrenColor()
    {
        ChangeColorScript[] children = transform.GetComponentsInChildren<ChangeColorScript>();
        foreach (ChangeColorScript child in children)
        {
            child.RestoreColor();
        }
    }

    public void ChangePosition()
    {

        if (player.transform.position.y < (teleportPosition.y -1.0f))
        {
            startPosition = player.transform.position;
            player.transform.position = teleportPosition;
        }
        else {
            player.transform.position = startPosition;
        }
        disableHighlight();
    }
}
