using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpacityBasedOnDistance : MonoBehaviour
{
    public float near = 5;
    public float far = 7;
    public Image img;
    public TMPro.TMP_Text[] texts;
    void OnEnable()
    {
        img = GetComponentInChildren<Image>();
        texts = GetComponentsInChildren<TMPro.TMP_Text>();
    }
    void Update()
    {
        var pos = RoverController.instance.transform.position;
        float opacity = 1 - Mathf.Clamp01(((pos - transform.position).magnitude - near) / (far - near));
        foreach (var text in texts) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, opacity);
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, opacity);
    }
}
