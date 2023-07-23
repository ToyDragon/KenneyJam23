using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCutsceneController : MonoBehaviour
{
    public static CamCutsceneController instance;
    public CameraFollow cameraFollow;
    public Transform posA;
    public Transform posB;
    public Transform lookPos;
    public Camera cam;
    private float oldFOV;
    public GameObject explosionEffects;
    public int state = 0;
    public bool running = true;
    public List<GameObject> enableOnDone = new List<GameObject>();
    public List<GameObject> destroyOnDone = new List<GameObject>();
    public float? nextStateTimer;
    private AudioSource audioSource;
    public AudioClip explosionClip;
    public AudioClip screamClip;
    public Transform astronautsRoot;
    public Transform roverRoot;
    public Transform roverLandingSpot;
    public Transform landingSpot;
    private Vector3 astronautStart;
    public GameObject uiWindow;
    public TMPro.TMP_Text uiText;
    public GameObject uiFullScreen;
    void OnEnable() {
        instance = this;
        if (!running) {
            return;
        }
        transform.position = posA.position;
        transform.LookAt(lookPos);
        cam = GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        oldFOV = cam.fieldOfView;
        cam.fieldOfView = 10;
        state = -1;
    }
    void Update()
    {
        if (!running) {
            return;
        }
        if (nextStateTimer.HasValue) {
            nextStateTimer -= Time.deltaTime;
            float t = (1.5f - nextStateTimer.Value) / 1.5f;
            if (state == 4) {
                astronautsRoot.position = astronautStart + (landingSpot.position - astronautStart) * t + Vector3.up * Mathf.Sin(t * Mathf.PI);
            }
            if (state == 5) {
                roverRoot.position = astronautStart + (roverLandingSpot.position - astronautStart) * t + Vector3.up * Mathf.Sin(t * Mathf.PI);
            }
            if (nextStateTimer <= 0) {
                nextStateTimer = null;
                state++;
            } else {
                return;
            }
        }
        if (state == -1) {
            bool skipCheck = false;
#if UNITY_EDITOR
            skipCheck = true;
#endif
            if (!skipCheck && Screen.width < 1500) {
                uiText.enabled = false;
                uiFullScreen.SetActive(true);
            } else {
                uiText.enabled = true;
                uiFullScreen.SetActive(false);
                if (Input.GetKeyDown(KeyCode.Space)) {
                    state = 0;
                    uiWindow.SetActive(false);
                    uiFullScreen.SetActive(false);
                }
            }
            return;
        }
        if (state == 0) {
            audioSource.PlayOneShot(explosionClip);
            nextStateTimer = 1.3f;
            state = 1;
            return;
        }
        if (state == 2) {
            nextStateTimer = .2f;
            return;
        }
        if (state == 3) {
            explosionEffects.SetActive(true);
            nextStateTimer = .5f;
            return;
        }
        if (state == 4) {
            audioSource.PlayOneShot(screamClip);
            astronautStart = astronautsRoot.position;
            nextStateTimer = 1.5f;
            transform.position = posB.position;
            transform.LookAt(astronautStart);
            cam.fieldOfView = oldFOV;
            return;
        }
        if (state == 5) {
            explosionEffects.SetActive(false);
            transform.position = posA.position;
            transform.LookAt(astronautStart);
            cam.fieldOfView = 10f;
            nextStateTimer = 1.5f;
            astronautStart = roverRoot.position;
            return;
        }
        if (state == 6) {
            cam.fieldOfView = oldFOV;
            foreach (var obj in enableOnDone) {
                obj.SetActive(true);
            }
            foreach (var obj in destroyOnDone) {
                Destroy(obj);
            }
            Destroy(roverRoot.gameObject);
            running = false;
        }
    }
}
