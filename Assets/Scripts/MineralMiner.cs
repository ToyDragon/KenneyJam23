using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMiner : MonoBehaviour
{
    public bool mining = false;
    float mineStartTime = 0f;
    int mineState = 0;
    public AudioSource audioSource;
    public List<AudioClip> drillSounds = new List<AudioClip>();
    public List<AudioClip> claySounds = new List<AudioClip>();
    public List<AudioClip> digSounds = new List<AudioClip>();
    public GameObject beingMined = null;
    private GameObject CheckForMineral(Vector3 pos, Vector3 forward) {
        Debug.DrawLine(pos, pos + forward * 1.5f, Color.blue);
        if (Physics.Raycast(pos, forward, out var hit, 1.5f)) {
            if (hit.transform.GetComponent<Mineral>() != null) {
                Debug.DrawLine(pos, pos + Vector3.up * .25f, Color.red);
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    void Update(){
        if(mining){
            float t = Time.time - mineStartTime;

            float vol = 2.5f;
            if (mineState == 0) { audioSource.PlayOneShot(drillSounds[0], vol); mineState++; }
            if (mineState == 1 && t >= .2f) { audioSource.PlayOneShot(claySounds[0], vol); mineState++; }
            if (mineState == 2 && t >= .5f) { audioSource.PlayOneShot(digSounds[0], vol); mineState++; }
            if (mineState == 3 && t >= .65f) { audioSource.PlayOneShot(claySounds[1], vol); mineState++; }
            if (mineState == 4 && t >= .95f) { audioSource.PlayOneShot(digSounds[1], vol); mineState++; }
            if (mineState == 5 && t >= 1.3f) { audioSource.PlayOneShot(drillSounds[1], vol); mineState++; }
            if (mineState == 6 && t >= 1.6f) { audioSource.PlayOneShot(digSounds[2], vol); mineState++; }
            if (mineState == 7 && t >= 2.2f) { audioSource.PlayOneShot(claySounds[2], vol); mineState++; }

            if (t >= 2.8f) {
                RoverController.instance.roverAnimator.SetBool("Digging", false);
            }

            if(t >= 3){
                if(beingMined != null){
                    GameObject.Destroy(beingMined.gameObject);
                    mining = false;
                    PIPDisplay.instance.img.enabled = false;
                    beingMined = null;
                    Crystal.Toggle();
                }
            }
            return;
        }

        var start = transform.position + Vector3.up * .1f;
        GameObject mineral = CheckForMineral(start, transform.forward);
        if(mineral != null && Input.GetKeyUp(KeyCode.Space)){
            if(transform.Find("Trailer") == null){
                ChatManager.instance.ShowAndStartText("Mining requires a trailer to store the resource!");
                return;
            }
            mining = true;
            mineStartTime = Time.time;
            mineState = 0;
            beingMined = mineral;
            RoverController.instance.roverAnimator.SetBool("Digging", true);
            PIPDisplay.instance.img.enabled = true;
        }
    }
}