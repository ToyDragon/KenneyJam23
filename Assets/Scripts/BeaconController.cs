using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconController : MonoBehaviour
{
    public GameObject idleShine;
    public GameObject lowIdleShine;
    public GameObject activatedShine;
    public AudioSource audioSource;
    public AudioClip activatedClip;
    public List<GameObject> hideOnActivate = new List<GameObject>();
    public List<GameObject> showOnActivate = new List<GameObject>();
    public int state = 0;
    public int beaconNumber = -1;
    public int objectiveNumber = -1;
    void OnEnable() {
        audioSource = GetComponent<AudioSource>();
        state = 1;
        idleShine.SetActive(true);
        lowIdleShine.SetActive(true);
        activatedShine.SetActive(false);
    }
    void Update()
    {
        float dist = (RoverController.instance.transform.position - transform.position).magnitude;
        if (dist < 4 && state == 1 && Input.GetKeyDown(KeyCode.Space)) {
            state = 2;
            idleShine.SetActive(false);
            lowIdleShine.SetActive(false);
            activatedShine.SetActive(true);
            audioSource.PlayOneShot(activatedClip);

            foreach (var obj in hideOnActivate) {
                obj.SetActive(false);
            }
            foreach (var obj in showOnActivate) {
                obj.SetActive(true);
            }

            WinConditions.instance.ActivateTower(beaconNumber);
            MissionObjectiveManager.instance.CompleteObjective(objectiveNumber);
        }
    }
}
