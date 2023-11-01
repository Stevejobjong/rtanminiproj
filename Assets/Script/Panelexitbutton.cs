using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panelexitbutton : MonoBehaviour
{
    public PanelManager panelManager;
    void Start()
    {
        panelManager = this.transform.parent.transform.parent.GetComponent<PanelManager>();
    }

    void Update()
    {
        
    }

    public void Exitbutton()
    {
        panelManager.Exidbutton();
        transform.parent.gameObject.SetActive(false);
    }

}
