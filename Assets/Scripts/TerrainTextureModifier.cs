using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTextureModifier : MonoBehaviour
{
    public static TerrainTextureModifier instance;
    public Texture noiseTexture;
    public Shader newShader;
    public Shader newGlowingShader;
    private int refreshMeshCountdown = 0;
    public Dictionary<string, Material> keyToModifiedMaterial = new Dictionary<string, Material>();
    public Dictionary<string, Material> keyToGlowingMaterial = new Dictionary<string, Material>();
    public Dictionary<string, Mesh> meshNameToFirstMesh = new Dictionary<string, Mesh>();
    public class RenderComponents {
        public Renderer renderer;
        public MeshFilter filter;
        public string meshKey;
    };
    private Dictionary<GameObject, RenderComponents> tToRenderComps = new Dictionary<GameObject, RenderComponents>();
    private List<GameObject> displayObjects = new List<GameObject>();
    public Material GetModifiedMaterial(Material original) {
        if (!original) { return null; }
        Material modifiedMaterial = null;
        if (keyToModifiedMaterial.TryGetValue(original.name, out modifiedMaterial)) {
            return modifiedMaterial;
        }
        modifiedMaterial = Material.Instantiate<Material>(original);
        modifiedMaterial.shader = newShader;
        modifiedMaterial.enableInstancing = true;
        modifiedMaterial.SetTexture("_NoiseTex", noiseTexture);
        keyToModifiedMaterial[original.name] = modifiedMaterial;
        return modifiedMaterial;
    }
    public Material GetGlowingMaterial(Material original) {
        if (!original) { return null; }
        Material modifiedMaterial = null;
        if (keyToGlowingMaterial.TryGetValue(original.name, out modifiedMaterial)) {
            return modifiedMaterial;
        }
        modifiedMaterial = Material.Instantiate<Material>(original);
        modifiedMaterial.shader = newGlowingShader;
        modifiedMaterial.enableInstancing = true;
        modifiedMaterial.SetTexture("_NoiseTex", noiseTexture);
        keyToGlowingMaterial[original.name] = modifiedMaterial;
        return modifiedMaterial;
    }
    public void MeshesChanged() {
        refreshMeshCountdown = 2;
    }
    void Update() {
        if (refreshMeshCountdown > 0) {
            refreshMeshCountdown--;
            if (refreshMeshCountdown == 0) {
                RegenerateDisplayMeshes();
            }
        }
    }
    private void RegenerateDisplayMeshes() {
        foreach (var dobj in displayObjects) {
            Destroy(dobj);
        }
        displayObjects.Clear();

        List<Material> rendererMaterials = new List<Material>();
        Dictionary<Material, List<CombineInstance>> matToMeshes = new Dictionary<Material, List<CombineInstance>>();

        for (int i = 0; i < transform.childCount; i++) {
            var terrain = transform.GetChild(i).gameObject;
            if (!terrain) {
                continue;
            }
            if (!tToRenderComps.TryGetValue(terrain, out var renderComponents)) {
                if (terrain.name.EndsWith("Indicator") || terrain.name.StartsWith("Batch ") || terrain.name.EndsWith("Nobatch")) {
                    continue;
                }
                renderComponents = tToRenderComps[terrain] = new RenderComponents() {
                    filter = terrain.GetComponentInChildren<MeshFilter>(),
                    renderer = terrain.GetComponentInChildren<Renderer>(),
                    meshKey = null,
                };
                renderComponents.renderer.enabled = false;
            }
            if (renderComponents.meshKey == null) {
                renderComponents.meshKey = renderComponents.filter.sharedMesh.name.Split(".")[0];
                var size = renderComponents.filter.sharedMesh.bounds.max - renderComponents.filter.sharedMesh.bounds.min;
                renderComponents.meshKey += "_" + Mathf.RoundToInt(size.x * 1000f) + "_" + Mathf.RoundToInt(size.y * 1000f) + "_" + Mathf.RoundToInt(size.z * 1000f) + "_" + renderComponents.filter.sharedMesh.vertices[0];
                if (meshNameToFirstMesh.TryGetValue(renderComponents.meshKey, out var existingMesh)) {
                    renderComponents.filter.sharedMesh = existingMesh;
                } else {
                    meshNameToFirstMesh[renderComponents.meshKey] = renderComponents.filter.sharedMesh;
                }
            }

            renderComponents.renderer.GetSharedMaterials(rendererMaterials);
            for (int j = 0; j < rendererMaterials.Count; j++) {
                if (!rendererMaterials[j]) { continue; }
                rendererMaterials[j] = GetModifiedMaterial(rendererMaterials[j]);
                if (!matToMeshes.ContainsKey(rendererMaterials[j])) {
                    matToMeshes[rendererMaterials[j]] = new List<CombineInstance>();
                }
                matToMeshes[rendererMaterials[j]].Add(new CombineInstance() {
                    mesh = meshNameToFirstMesh[renderComponents.meshKey],
                    transform = renderComponents.renderer.transform.localToWorldMatrix,
                    subMeshIndex = j,
                });
            }
            renderComponents.renderer.enabled = false;
        }

        foreach (var matAndInstance in matToMeshes) {
            GameObject batchObj = new GameObject("Batch " + matAndInstance.Key.name);
            Mesh m = new Mesh();
            m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            m.CombineMeshes(matAndInstance.Value.ToArray());
            batchObj.AddComponent<MeshFilter>().sharedMesh = m;
            batchObj.AddComponent<MeshRenderer>().sharedMaterial = matAndInstance.Key;
            batchObj.transform.SetParent(transform);
            displayObjects.Add(batchObj);
        }
    }
    void OnEnable()
    {
        instance = this;
        for (int i = 0; i < transform.childCount; i++) {
            var terrain = transform.GetChild(i).gameObject;
            if (!terrain) {
                continue;
            }
            var renderComponents = tToRenderComps[terrain] = new RenderComponents() {
                filter = terrain.GetComponent<MeshFilter>(),
                renderer = terrain.GetComponent<Renderer>(),
            };
            if (!terrain.TryGetComponent<MeshCollider>(out var collider)) {
                collider = terrain.AddComponent<MeshCollider>();
            }
            string meshkey = renderComponents.filter.sharedMesh.name.Split(".")[0];
            var size = renderComponents.filter.sharedMesh.bounds.max - renderComponents.filter.sharedMesh.bounds.min;
            meshkey += "_" + Mathf.RoundToInt(size.x * 1000f) + "_" + Mathf.RoundToInt(size.y * 1000f) + "_" + Mathf.RoundToInt(size.z * 1000f) + "_" + renderComponents.filter.sharedMesh.vertices[0];
            if (meshNameToFirstMesh.TryGetValue(meshkey, out var existingMesh)) {
                renderComponents.filter.sharedMesh = existingMesh;
                collider.sharedMesh = existingMesh;
            } else {
                meshNameToFirstMesh[meshkey] = renderComponents.filter.sharedMesh;
                collider.sharedMesh = renderComponents.filter.sharedMesh;
            }
            renderComponents.renderer.enabled = false;
        }
        RegenerateDisplayMeshes();
    }
}
