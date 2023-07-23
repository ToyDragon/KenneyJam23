using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirtinator : MonoBehaviour
{
    void Start() {
        var renderers = GetComponentsInChildren<Renderer>();
        List<Material> rendererMaterials = new List<Material>();
        foreach (var renderer in renderers) {
            renderer.GetSharedMaterials(rendererMaterials);
            for (int j = 0; j < rendererMaterials.Count; j++) {
                if (!rendererMaterials[j]) { continue; }
                rendererMaterials[j] = TerrainTextureModifier.instance.GetModifiedMaterial(rendererMaterials[j]);
            }
            renderer.SetSharedMaterials(rendererMaterials);
        }
    }
}
