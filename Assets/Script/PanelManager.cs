using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject[] memberPanel = new GameObject[5];
    public bool panelopen = false;

    public GameObject cards;

    void Start()
    {
        Startpanel();
    }

    void Update()
    {
        
    }

    public void Startpanel()
    {
        int numofchild = this.transform.childCount;
        for (int i = 0; i < numofchild; i++)
        {
            memberPanel[i] = transform.GetChild(i).gameObject;
        }
    }

    public void Openpanel(int a)
    {
        memberPanel[a].gameObject.SetActive(true);
        panelopen = true;
        PSetAc();
    }

    public void Exidbutton()
    {       
        panelopen = false;
        PSetAc();
    }

    void PSetAc()
    {
        if(panelopen)
        {
            cards.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            cards.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
