using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialLight : MonoBehaviour
{
    public static List<InitialLight> initialLights = new List<InitialLight>();
    void OnEnable() {
        GetComponent<Renderer>().enabled = false;
        initialLights.Add(this);
    }
}
