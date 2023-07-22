using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampCreator : MonoBehaviour
{
    public GameObject rampPrefab;
    public GameObject glowingObject = null;
    public GameObject lastHitObject = null;
    private List<Material> rendererMaterials = new List<Material>();
    void Update()
    {
        var start = transform.position + Vector3.up * .3f;
        Debug.DrawLine(start, start + transform.forward*1.5f);
        GameObject hitObject = null;
        if (Physics.Raycast(start, transform.forward, out var hit, 1.5f)) {
            if (hit.transform.gameObject.name.StartsWith("terrain_sideCliff")) {
                hitObject = hit.transform.gameObject;
            }
        }

        if (hitObject != lastHitObject) {
            lastHitObject = hitObject;
            if (glowingObject != null) {
                Destroy(glowingObject);
            }
            if (hitObject) {
                glowingObject = GameObject.Instantiate(hitObject);
                if (glowingObject.TryGetComponent<MeshCollider>(out var collider)) {
                    Destroy(collider);
                }
                if (glowingObject.TryGetComponent<Renderer>(out var renderer)) {
                    renderer.GetSharedMaterials(rendererMaterials);
                    for (int j = 0; j < rendererMaterials.Count; j++) {
                        rendererMaterials[j] = TerrainTextureModifier.instance.GetGlowingMaterial(rendererMaterials[j]);
                    }
                    renderer.SetSharedMaterials(rendererMaterials);
                }
            }
        }

        if (hitObject && Input.GetKeyDown(KeyCode.Space)) {
            var newRamp = GameObject.Instantiate(rampPrefab);
            newRamp.transform.SetParent(hitObject.transform.parent);
            newRamp.transform.rotation = hitObject.transform.rotation;
            newRamp.transform.position = hitObject.transform.position - hitObject.transform.forward * .5f + hitObject.transform.right * .5f;
            if (newRamp.transform.childCount == 1 && newRamp.transform.GetChild(0).TryGetComponent<Renderer>(out var renderer)) {
                renderer.GetSharedMaterials(rendererMaterials);
                for (int j = 0; j < rendererMaterials.Count; j++) {
                    rendererMaterials[j] = TerrainTextureModifier.instance.GetModifiedMaterial(rendererMaterials[j]);
                }
                renderer.SetSharedMaterials(rendererMaterials);
            }
            Destroy(glowingObject);
            glowingObject = null;
            Destroy(hitObject);
            hitObject = null;
        }
    }
}
