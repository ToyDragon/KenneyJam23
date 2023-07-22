using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampCreator : MonoBehaviour
{
    public static RampCreator instance;
    public GameObject rampPrefab;
    public GameObject glowingObject = null;
    public GameObject lastHitObject = null;
    private List<Material> rendererMaterials = new List<Material>();
    public float? rampCreationStart = null;
    public List<AudioClip> drillSounds = new List<AudioClip>();
    public List<AudioClip> claySounds = new List<AudioClip>();
    public List<AudioClip> digSounds = new List<AudioClip>();
    public List<AudioClip> tossSounds = new List<AudioClip>();
    public AudioSource audioSource;
    public int rampCreationState = 0;
    void OnEnable() {
        instance = this;
    }
    private bool CheckForRamp(Vector3 pos, Vector3 forward) {
        Debug.DrawLine(pos, pos + forward * 1.5f, Color.blue);
        if (Physics.Raycast(pos, forward, out var hit, 1.5f)) {
            if (hit.transform.gameObject.name.StartsWith("terrain_ramp")) {
                return true;
            }
        }
        return false;
    }
    void Update()
    {
        if (rampCreationStart.HasValue) {
            if (!lastHitObject) {
                rampCreationStart = null;
                return;
            }
            float t = Time.time - rampCreationStart.Value;

            float vol = 2.5f;
            if (rampCreationState == 0) { audioSource.PlayOneShot(drillSounds[0], vol); rampCreationState++; }
            if (rampCreationState == 1 && t >= .2f) { audioSource.PlayOneShot(claySounds[0], vol); rampCreationState++; }
            if (rampCreationState == 2 && t >= .5f) { audioSource.PlayOneShot(digSounds[0], vol); rampCreationState++; }
            if (rampCreationState == 3 && t >= .65f) { audioSource.PlayOneShot(claySounds[1], vol); rampCreationState++; }
            if (rampCreationState == 4 && t >= .95f) { audioSource.PlayOneShot(digSounds[1], vol); rampCreationState++; }
            if (rampCreationState == 5 && t >= 1.3f) { audioSource.PlayOneShot(drillSounds[1], vol); rampCreationState++; }
            if (rampCreationState == 6 && t >= 1.6f) { audioSource.PlayOneShot(digSounds[2], vol); rampCreationState++; }
            if (rampCreationState == 7 && t >= 2.2f) { audioSource.PlayOneShot(claySounds[2], vol); rampCreationState++; }
            
            if (t >= 3) {
                var newRamp = GameObject.Instantiate(rampPrefab);
                newRamp.transform.SetParent(lastHitObject.transform.parent);
                newRamp.transform.rotation = lastHitObject.transform.rotation;
                newRamp.transform.position = lastHitObject.transform.position - lastHitObject.transform.forward * .5f + lastHitObject.transform.right * .5f;
                if (newRamp.transform.childCount == 1 && newRamp.transform.GetChild(0).TryGetComponent<Renderer>(out var renderer)) {
                    renderer.GetSharedMaterials(rendererMaterials);
                    for (int j = 0; j < rendererMaterials.Count; j++) {
                        rendererMaterials[j] = TerrainTextureModifier.instance.GetModifiedMaterial(rendererMaterials[j]);
                    }
                    renderer.SetSharedMaterials(rendererMaterials);
                }
                Destroy(glowingObject);
                glowingObject = null;
                Destroy(lastHitObject);
                lastHitObject = null;
                rampCreationStart = null;
            }
            return;
        }

        var start = transform.position + Vector3.up * .3f;
        Debug.DrawLine(start, start + transform.forward*1.5f);
        GameObject hitObject = null;
        if (Physics.Raycast(start, transform.forward, out var hit, 1.5f)) {
            if (hit.transform.gameObject.name.StartsWith("terrain_sideCliff")) {
                if (hit.transform.gameObject.TryGetComponent<MeshFilter>(out var filter)) {
                    float height = filter.sharedMesh.bounds.max.y - filter.sharedMesh.bounds.min.y;
                    if (height < 0.006f) {
                        hitObject = hit.transform.gameObject;
                    }
                }
            }
            var center = hit.transform.position + hit.transform.right*.5f + hit.transform.up * .25f;
            bool neighboringRamp = CheckForRamp(center + hit.transform.right, -hit.transform.forward*.5f) || CheckForRamp(center - hit.transform.right, -hit.transform.forward*.5f);
            if (neighboringRamp) {
                hitObject = null;
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
                
                float offset = .01f;
                glowingObject.transform.position += glowingObject.transform.forward * offset + glowingObject.transform.up * offset;
            }
        }

        if (hitObject && Input.GetKeyDown(KeyCode.Space)) {
            rampCreationStart = Time.time;
            rampCreationState = 0;
        }
    }
}
