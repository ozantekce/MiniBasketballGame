using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Panel
{
    public string name;
    public GameObject panel;


    public static Dictionary<string, GameObject> CreatePanelsDictionary(Panel[] panelsArray)
    {
        Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
        for (int i = 0; i < panelsArray.Length; i++)
        {
            panels[panelsArray[i].name] = panelsArray[i].panel;
        }

        return panels;

    }

    private static GameObject currentPanel;
    public static void ChangePanel(Dictionary<string, GameObject> panels,string panelName)
    {

        if (currentPanel != null)
            currentPanel.SetActive(false);

        currentPanel = panels[panelName];
        currentPanel.SetActive(true);


    }

}




