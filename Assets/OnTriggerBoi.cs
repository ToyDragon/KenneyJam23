using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerBoi : MonoBehaviour
{
    HashSet<string> namesOfTriggered = new HashSet<string>();
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collision){
        if(collision.gameObject.GetComponent<TriggerBoi>() != null){
            if(!namesOfTriggered.Contains(collision.gameObject.name)){
                Debug.Log("Completing objective");
                MissionObjectiveManager.instance.CompleteCurrentObjective();
                namesOfTriggered.Add(collision.gameObject.name);
            }
        }
    }
}
