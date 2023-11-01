using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class PanelManager : MonoBehaviour
{
    public static PanelManager i;

    public GameObject[] memberPanel = new GameObject[5];
    public bool panelopen = false;

    public GameObject cards;
    public GameObject backcanvas;

    public GameObject button;
    public GameObject[] buttons = new GameObject[5];

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        Startpanel();
        int a = button.transform.childCount;
        for (int i = 0; i < a; i++)
        {
            buttons[i] = button.transform.GetChild(i).gameObject;
            buttons[i].transform.GetComponent<Openbutton>().mynum = i;
                
        }
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
        buttons[a].gameObject.SetActive(true);
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
            backcanvas.SetActive(false);
            cards.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            backcanvas.SetActive(true);
            cards.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
