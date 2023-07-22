using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarMgr : MonoBehaviour
{
    public Texture2D fogOfWarTexture;
    public Vector2Int lastGrid;
    public Color fogColor = Color.black;
    void OnEnable()
    {
        fogOfWarTexture = new Texture2D(1024, 1024);
        for (int x = 0; x < 1024; x++) {
            for (int y = 0; y < 1024; y++) {
                fogOfWarTexture.SetPixel(x, y, Color.black);
            }
        }
        fogOfWarTexture.Apply();
    }

    void Update()
    {
        foreach (var mat in CustomPPRendererFeature.instance.colorBlitMaterials) {
            mat.SetColor("_FogColor", fogColor);
            mat.SetTexture("_FogOfWarTexture", fogOfWarTexture);
            mat.SetVector("_FeaturePosition", RoverController.instance.transform.position);
        }
        var gridPos = new Vector2Int(Mathf.RoundToInt(RoverController.instance.transform.position.x), Mathf.RoundToInt(RoverController.instance.transform.position.z));
        if (gridPos != lastGrid) {
            lastGrid = gridPos;
            for (int dx = -15; dx <= 15; dx++) {
                for (int dy = -15; dy <= 15; dy++) {
                    var offsetPos = gridPos + new Vector2Int(dx, dy) + new Vector2Int(512, 512);
                    if (offsetPos.x < 0 || offsetPos.x >= 1024 || offsetPos.y < 0 || offsetPos.y >= 1024) {
                        continue;
                    }
                    fogOfWarTexture.SetPixel(offsetPos.x, offsetPos.y,
                        Color.white * Mathf.Max(
                            Mathf.Clamp01(.7f - (new Vector2Int(dx, dy).magnitude / 15f)),
                            fogOfWarTexture.GetPixel(offsetPos.x, offsetPos.y).r
                    ));
                }
            }
            fogOfWarTexture.Apply();
        }
    }
}