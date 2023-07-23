using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickup : MonoBehaviour
{
    public Vector3 startPos;
    public AudioClip getClip;
    void OnEnable()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * 1.5f) * .25f;
        transform.localEulerAngles = Vector3.up * Time.time * 120;
    }
    void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<RoverController>(out var rover)) {
            rover.hasLeadArmor = true;
            Destroy(gameObject);
            RampCreator.instance.audioSource.PlayOneShot(getClip);
            ItemGetToaster.instance.Show("Protection from radiation");
            MissionObjectiveManager.instance.CompleteObjective(5);
        }
    }
}
