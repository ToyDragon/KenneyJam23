using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        masterSlider.onValueChanged.AddListener(delegate { ApplyVolumeChanges(); });
        musicSlider.onValueChanged.AddListener(delegate { ApplyVolumeChanges(); });
        sfxSlider.onValueChanged.AddListener(delegate { ApplyVolumeChanges(); });

        ApplyVolumeChanges();
    }

    void ApplyVolumeChanges(){
        audioMixer.SetFloat("volMaster", masterSlider.value == 0f ? -80f : masterSlider.value * 20f - 20f);
        audioMixer.SetFloat("volMusic", musicSlider.value == 0f ? -80f : musicSlider.value * 20f - 20f);
        audioMixer.SetFloat("volSFX", sfxSlider.value == 0f ? -80f : sfxSlider.value * 20f - 20f);
    }
}
