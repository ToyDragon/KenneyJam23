using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionObjectiveManager : MonoBehaviour
{
    public static MissionObjectiveManager instance;
    Objective[] objectives;
    int currentRootObjective = 11;

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
        //0
        Objective activateMazeBeacon = new Objective("Activate the maze beacon", 1);
        //1
        Objective getTrailer = new Objective("Retrieve the trailer", 2);
        //2
        Objective getMarsium = new Objective("Collect 3 marsium", 2);
        //3
        Objective deliverMarsium = new Objective("Deliver marsium to the bridge", 2);
        //4
        Objective activateBridgeBeacon = new Objective("Activate the bridge beacon", 1, 
            new Objective[]{getTrailer,getMarsium,deliverMarsium});
        //5
        Objective leadArmor = new Objective("Pick up lead armor", 2);
        //6
        Objective activateRadiatedBeacon = new Objective("Activate the radiated beacon", 1, 
            new Objective[]{leadArmor});
        //7
        Objective mainObjective = new Objective("Active the 3 remaining beacons", 0, 
            new Objective[]{activateMazeBeacon,activateBridgeBeacon,activateRadiatedBeacon});
        //8
        Objective approachAstronauts = new Objective("Approach the astronauts", 1);
        //9
        Objective activateFirstBeacon = new Objective("Activate the nearby beacon", 1);
        //10
        Objective grabTool = new Objective("Grab the ROV-ER multi-tool", 1);
        //11
        Objective escapeCrater = new Objective("Escape the crater", 0, 
            new Objective[]{approachAstronauts,activateFirstBeacon,grabTool});

        objectives = new Objective[]{
            activateMazeBeacon,
            getTrailer,
            getMarsium,
            deliverMarsium,
            activateBridgeBeacon,
            leadArmor,
            activateRadiatedBeacon,
            mainObjective,
            approachAstronauts,
            activateFirstBeacon,
            grabTool,
            escapeCrater
        };

        RefreshGUI();
    }

    public void CompleteObjective(int objective){
        objectives[objective].completed = true;
        if(objective == 11) currentRootObjective = 7;
        RefreshGUI();
    }

    public string BuildObjectiveText(Objective objective){
        string toRet = objective.objectiveText + "\n";
        if(objective.subobjectives != null){
            foreach(var child in objective.subobjectives){
                if(child.completed) toRet += "(DONE)";
                toRet += BuildObjectiveText(child);
            }
        }
        return toRet;
    }

    void RefreshGUI(){
        string text = BuildObjectiveText(objectives[currentRootObjective]);
        int numLines = text.Split('\n').Length;
        Debug.Log("Number of lines is "+ numLines);
        GetComponent<TextMeshProUGUI>().text = text;
        RectTransform rect = transform.parent.GetComponent<RectTransform>();
        //rect.sizeDelta = new Vector2(256f, );
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,0f,(numLines+1f)*18.0f);
        //rect.position = new Vector2(rect.position.x, -(numLines+1f)*22.0f/2);
    }

    void OnEnable(){
        instance = this;
    }
}

public class Objective{
    public bool completed = false;
    public string objectiveText;
    public List<Objective> subobjectives = null;

    public Objective(string text, int depth){
        objectiveText = text;
        for(int i = 0; i < depth; i++){
            objectiveText = "    " + objectiveText;
        }
    }
    public Objective(string text, int depth, Objective[] subobjectives){
        objectiveText = text;
        this.subobjectives = new List<Objective>(subobjectives);
        for(int i = 0; i < depth; i++){
            objectiveText = "    " + objectiveText;
        }
    }
}
