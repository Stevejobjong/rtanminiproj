using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour {
    public static Cam instance;

    void Start() {
        Screen.SetResolution(760, 1280, false);
    }

}
