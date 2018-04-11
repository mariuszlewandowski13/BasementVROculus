using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

public enum OperationType
{
    Search = 1,
    Back = 2
}

public class BrowserButtonScript : MonoBehaviour, IPointable
{
    public GameObject browser;

    public OperationType operation;

    private bool highlighted;


    private void OnTriggerDown()
    {
        if (browser != null)
        {
            if (operation == OperationType.Search)
            {
                browser.GetComponent<SearchNewWebPageScript>().StartSearch();
            }
            else if (operation == OperationType.Back )
            {
                browser.GetComponent<Browser>().GoBack();
            }
            
        }
    }

    public void PointerIn()
    {
        if (!highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOn(ApplicationStaticData.toolsHighligthing);
            highlighted = true;
        }
    }

    public void PointerOut()
    {
        if (highlighted)
        {
            GetComponent<HighlightingSystem.Highlighter>().ConstantOff();
            highlighted = false;
        }
    }


    public void TriggerDown()
    {
        OnTriggerDown();
    }

}
