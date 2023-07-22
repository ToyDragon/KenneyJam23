using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureModifier : MonoBehaviour
{
    public static TerrainTextureModifier instance;
    public Texture noiseTexture;
    public Shader newShader;
    public Shader newGlowingShader;
    public Dictionary<string, Material> keyToModifiedMaterial = new Dictionary<string, Material>();
    public Dictionary<string, Material> keyToGlowingMaterial = new Dictionary<string, Material>();
    public Material GetModifiedMaterial(Material original) {
        Material modifiedMaterial = null;
        if (keyToModifiedMaterial.TryGetValue(original.name, out modifiedMaterial)) {
            return modifiedMaterial;
        }
        modifiedMaterial = Material.Instantiate<Material>(original);
        modifiedMaterial.shader = newShader;
        modifiedMaterial.SetTexture("_NoiseTex", noiseTexture);
        keyToModifiedMaterial[original.name] = modifiedMaterial;
        return modifiedMaterial;
    }
    public Material GetGlowingMaterial(Material original) {
        Material modifiedMaterial = null;
        if (keyToModifiedMaterial.TryGetValue(original.name, out modifiedMaterial)) {
            return modifiedMaterial;
        }
        modifiedMaterial = Material.Instantiate<Material>(original);
        modifiedMaterial.shader = newGlowingShader;
        modifiedMaterial.SetTexture("_NoiseTex", noiseTexture);
        keyToModifiedMaterial[original.name] = modifiedMaterial;
        return modifiedMaterial;
    }
    void OnEnable()
    {
        instance = this;
        List<Material> rendererMaterials = new List<Material>();
        for (int i = 0; i < transform.childCount; i++) {
            var terrain = transform.GetChild(i).gameObject;
            if (terrain && terrain.TryGetComponent<Renderer>(out var renderer)) {
                renderer.GetSharedMaterials(rendererMaterials);
                for (int j = 0; j < rendererMaterials.Count; j++) {
                    rendererMaterials[j] = GetModifiedMaterial(rendererMaterials[j]);
                }
                renderer.SetSharedMaterials(rendererMaterials);
                MeshCollider collider = terrain.AddComponent<MeshCollider>();
                collider.sharedMesh = terrain.GetComponent<MeshFilter>().mesh;
            }
        }
    }
}
