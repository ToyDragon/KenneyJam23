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
                MissionObjectiveManager.instance.CompleteObjective(
                    collision.gameObject.GetComponent<TriggerBoi>().objectiveCompleted);
                namesOfTriggered.Add(collision.gameObject.name);
                if(collision.gameObject.name == "MissionTrigger"){
                    ChatManager.instance.ShowAndStartText("Come in ROV-ER! This is captain Buzz of the Mars Exploration Task Force.\n\n"
        +"Our crew is out of supplies and we are currently experiencing a comms outage with systems command.\n\n"
        +"You need to reactivate the communication towers spread around the field operating base.\n\n"
        +"To get over there you will need to collect your ROV-ER Multi-Tool to help traverse the terrain.\n\n"
        +"Quickly please, I know not how much longer we will last.");
                }
            }
        } else if(collision.gameObject.GetComponent<DeadIfYouTouchThis>() != null){
            transform.parent.gameObject.GetComponent<CharacterController>().enabled = false;
            transform.parent.position = collision.gameObject.GetComponent<DeadIfYouTouchThis>().respawnPosition.position;
            transform.parent.gameObject.GetComponent<CharacterController>().enabled = true;
        }
    }
}
