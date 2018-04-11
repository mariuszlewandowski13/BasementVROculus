using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrowserHelperScript {

    public static List<string> PossibleInputFieldTypes = new List<string>() {"email" , "password", "tel", "text", "url", "number", "search" };
    public static string getInputsFieldsTypes = "OnActiveElementChange(document.activeElement.tagName, document.activeElement.value, document.activeElement.type);";
    public static string setOnDragFunctionOnAHref = "if(document.activeElement.tagName == 'A' && document.activeElement.firstChild != null && document.activeElement.firstChild.tagName == 'IMG' && !document.activeElement.classList.contains('basementDragClass')){document.activeElement.firstChild.draggable = true; document.activeElement.firstChild.ondragstart = OnStartDragFunction(document.activeElement.firstChild.src); document.activeElement.className += ' basementDragClass'}";
    public static string setOnDragFunctionOnImg = "if(document.activeElement.tagName == 'IMG' && !document.activeElement.classList.contains('basementDragClass')){document.activeElement.draggable = true; document.activeElement.ondragstart = OnStartDragFunction(document.activeElement.src); document.activeElement.className += ' basementDragClass'}";

    public static string forEachImgElementAddOnDragFunction = "for (i = 0; i < document.images.length; i++) { " +
        "if(!document.images[i].classList.contains('basementDragClass')){" +
        "document.images[i].addEventListener('onclick', OnStartDragFunction(document.images[i].src));" +
        "document.images[i].className += ' basementDragClass';" +
    "}}";
}
