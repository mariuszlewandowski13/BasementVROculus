using UnityEngine;

public class  DoorCollider:MonoBehaviour
{
    public bool collide;
    public bool positionCollider;
    public DoorOpenScript doorCollider;
    public ChangePositionScript changePositionScript;

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "door")
        {
           // Debug.Log("collide = true; ");
            doorCollider = target.GetComponent<DoorOpenScript>();
            doorCollider.highlightRed();
            collide = true;
        }

        if (target.tag == "position")
        {
            // Debug.Log("position = true; ");
            changePositionScript = target.GetComponent<ChangePositionScript>();
            changePositionScript.highlightRed();
            positionCollider = true;
        }
    }
    
    void OnTriggerExit(Collider target)
    {
        if (target.tag == "door")
        {
           // Debug.Log("collide = false; ");
            doorCollider = target.GetComponent<DoorOpenScript>();
            doorCollider.disableHighlight();
            collide = false;
        }

        if (target.tag == "position")
        {
            // Debug.Log("position = false; ");
            changePositionScript = target.GetComponent<ChangePositionScript>();
            changePositionScript.disableHighlight();
            positionCollider = false;
        }
    }
}