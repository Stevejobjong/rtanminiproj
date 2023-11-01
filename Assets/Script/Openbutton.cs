using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openbutton : MonoBehaviour
{
    public int mynum;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Callpanel()
    {
        PanelManager.i.Openpanel(mynum);
    }
}
