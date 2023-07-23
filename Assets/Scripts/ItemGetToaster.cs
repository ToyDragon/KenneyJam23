using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGetToaster : MonoBehaviour
{
    public static ItemGetToaster instance;
    public GameObject content;
    public float timeShown = -100;
    public float toastTime = 10f;
    public TMPro.TMP_Text text;
    public void Show(string t) {
        text.SetText(t);
        timeShown = Time.time;
    }
    void OnEnable() {
        instance = this;
        text = GetComponentInChildren<TMPro.TMP_Text>();
    }
    void Update()
    {
        content.SetActive(Time.time < timeShown + toastTime);
    }
}
