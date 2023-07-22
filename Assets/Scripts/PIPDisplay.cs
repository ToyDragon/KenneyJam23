using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PIPDisplay : MonoBehaviour
{
    public static PIPDisplay instance;
    public RawImage img;
    void OnEnable() {
        instance = this;
        img = GetComponent<RawImage>();
        img.enabled = false;
    }
}
