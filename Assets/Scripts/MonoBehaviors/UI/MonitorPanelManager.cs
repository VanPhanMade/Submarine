using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MonitorPanelManager : MonoBehaviour
{
    List<GameObject> panels = new List<GameObject>();
    GameObject activePanel = null;
    int panelIndex;
    private void Start()
    {

        panelIndex = 0;
        foreach (Transform panel in transform)
            panels.Add(panel.gameObject);
        activePanel = panels[panelIndex];
    }

    public void IncreasePanelIndex()
    {
        if (panelIndex + 1 < panels.Count)
        {
            panelIndex++;
        }
        else
            panelIndex = 0;

        SwitchPanels();
    }

    public void DecreasePanelIndex()
    {
        if(panelIndex - 1 > -1)
            panelIndex--;
        else
            panelIndex = panels.Count - 1;
        SwitchPanels();
    }
    
    void SwitchPanels()
    {
        activePanel.SetActive(false);
        activePanel = panels[panelIndex];
        activePanel.SetActive(true);

    }

}
