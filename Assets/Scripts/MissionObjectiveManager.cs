using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionObjectiveManager : MonoBehaviour
{
    public static MissionObjectiveManager instance;
    int numObjectives = 5;
    int currentObjective = -1;

    string[] objectives = new string[]{
        "Locate the ROV-ER Multi-Tool",
        "Escape the crater",
        "Locate and hitch the trailer",
        "Gather Marsium",
        "Reactivate the comms towers",
    };

    string[] dialogs = new string[]{
        "Come in ROV-ER! This is captain Buzz of the Mars Exploration Task Force.\n\n"
        +"Our crew is out of supplies and we are currently experiencing a comms outage with systems command.\n\n"
        +"You need to reactivate the communication towers spread around the field operating base.\n\n"
        +"To get over there you will need to collect your ROV-ER Multi-Tool to help traverse the terrain.\n\n"
        +"Quickly please, I know not how much longer we will last.",
        "",
        "",
        "",
        ""
    };
    // Start is called before the first frame update
    void Start()
    {
        // CompleteCurrentObjective();
    }

    void OnEnable(){
        instance = this;
    }

    public void SetObjective(string objective){
        GetComponent<TextMeshProUGUI>().text = objective;
    }

    public void CompleteCurrentObjective(){
        currentObjective++;
        if(currentObjective <= numObjectives-1){
            if(dialogs[currentObjective] != ""){
                ChatManager.instance.ShowAndStartText(dialogs[currentObjective]);
            }
            SetObjective(objectives[currentObjective]);
        }
    }
}
