using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honker : MonoBehaviour
{
    public List<AudioClip> clips;
    public float timePerHonk = 4;
    private AudioSource audioSource;
    public float lastHonk = 8;
    void OnEnable() {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Mathf.FloorToInt(Time.time / timePerHonk) > Mathf.FloorToInt(lastHonk / timePerHonk)) {
            audioSource.clip = clips[Random.Range(0, clips.Count)];
            audioSource.Play();
            lastHonk = Time.time;
        }
    }
}

