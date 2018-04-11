using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ImageMoveScript))]
public class DropScript : MonoBehaviour {


    private GameObject parentObject;

    public OVRInput.Controller controller;

    private GameObject lerpController;


    void Update () {
        if (parentObject != null)
        {
            parentObject.transform.position = lerpController.transform.position;
            parentObject.transform.rotation = lerpController.transform.rotation;

            if (gameObject.transform.parent != parentObject.transform)
            {
                if (GetComponent<PhotonView>().isMine)
                {
                    gameObject.transform.parent = parentObject.transform;
                    GetComponent<ImageMoveScript>().SaveParentByNetwork();
                    GetComponent<ImageMoveScript>().SetParentByNetwork(parentObject.GetComponent<PhotonView>().viewID);
                }
                else {
                    GetComponent<PhotonView>().RequestOwnership();
                }

            }


            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller))
            {
                gameObject.transform.parent = null;
                GetComponent<ImageMoveScript>().LoadParentByNetwork();
                SaveManager.SaveGameObject(gameObject);
                Destroy(this);
            }
        }
        else {
            Destroy(this);
        }
	}


    public void SetParentObject(GameObject newParent, GameObject controller)
    {
        lerpController = controller;
        parentObject = newParent;

        parentObject.transform.position = lerpController.transform.position;
        parentObject.transform.rotation = lerpController.transform.rotation;

        if (gameObject.transform.parent != parentObject.transform)
        {
            if (GetComponent<PhotonView>().isMine)
            {
                gameObject.transform.parent = parentObject.transform;
                GetComponent<ImageMoveScript>().SaveParentByNetwork();
                GetComponent<ImageMoveScript>().SetParentByNetwork(parentObject.GetComponent<PhotonView>().viewID);
            }
            else {
                GetComponent<PhotonView>().RequestOwnership();
            }

        }



    }



}
