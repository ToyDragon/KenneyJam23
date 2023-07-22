using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FeaturePoint : MonoBehaviour
{
  public static FeaturePoint instance;
  public Texture noiseTexture;
  public Texture2D distanceTexture;
  public RawImage distanceTextureDisplay;
  public int steps = 4;
  public float stepStrength = .5f;
  void OnEnable() {
    instance = this;
    distanceTexture = new Texture2D(64, 64);
    distanceTextureDisplay.texture = distanceTexture;
  }
  public int rayCount = 360;
  void Update() {
    if (CustomPPRendererFeature.instance == null) {
      return;
    }
    foreach (var mat in CustomPPRendererFeature.instance.colorBlitMaterials) {
      mat.SetInt("_RayCount", rayCount);
      mat.SetFloat("_StepCount", steps);
      mat.SetFloat("_StepStrength", stepStrength);
      mat.SetTexture("_NoiseTexture", noiseTexture);
      mat.SetTexture("_FeatureDepthTexture", distanceTexture);
      mat.SetVector("_FeaturePointWS", transform.position);
      mat.SetVector("_FeaturePointScreen", Camera.main.WorldToScreenPoint(transform.position));
      mat.SetVector("_FeaturePointWS", transform.position);
    }

    int texSize = 64;
    for (int i = 0; i < rayCount; i++) {
      float dist = 1;
      var start = transform.position;// + Vector3.up*.5f;
      if (Physics.Raycast(start, Quaternion.Euler(0, 360 * i / (float)rayCount, 0) * Vector3.forward, out var hit, 100f, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
        dist = Mathf.Clamp01((hit.point - start).magnitude / 100f);
      }
      distanceTexture.SetPixel(i % texSize, i / texSize, Color.white * dist);
    }
    distanceTexture.Apply();
  }
}
