using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadiationBehavior : MonoBehaviour
{
    public Transform radiationWarningImage;
    public AudioSource audioSource;
    public float health = 3;
    public float maxHealth = 3;
    public float opacity;
    public Image[] imgs;
    private void SetA(Image i, float a) {
        i.color = new Color(i.color.r, i.color.g, i.color.b, a);
    }
    void OnEnable() {
        health = maxHealth;
        imgs = radiationWarningImage.GetComponentsInChildren<Image>();
        foreach (var img in imgs) {
            SetA(img, 0);
        }
    }
    void Update()
    {
        Vector2Int grid = new Vector2Int(Mathf.FloorToInt(RoverController.instance.transform.position.x + 512), Mathf.FloorToInt(RoverController.instance.transform.position.z + 512));
        bool inRadiation = FogOfWarMgr.instance.radiationTexture.GetPixel(grid.x, grid.y).r > .5f;
        if (!RoverController.instance.hasLeadArmor && inRadiation) {
            health -= Time.deltaTime;
        } else {
            health = Mathf.Min(health + Time.deltaTime, 3f);
        }
        opacity = Mathf.Clamp01((maxHealth - health) / (maxHealth - 1));
        if (opacity > 0.1) {
            opacity = Mathf.Clamp01(opacity + Mathf.Sin(Time.time * 5)*.3f);
        }
        foreach (var img in imgs) {
            SetA(img, opacity);
        }
        float vol = Mathf.Clamp01((maxHealth - health) * 10f);
        if (inRadiation) {
            vol = Mathf.Max(vol, .2f);
        }
        audioSource.volume = vol;
        if (health <= 0) {
            CharacterController controller = RoverController.instance.GetComponent<CharacterController>();
            controller.enabled = false;
            RoverController.instance.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            controller.enabled = true;
            health = maxHealth;
        }
    }
}
