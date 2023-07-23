using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    public GameObject enableOnWin;
    public static WinConditions instance;
    bool[] activatedTowers = new bool[]{false,false,false,false};

    void OnEnable(){
        instance = this;
    }

    private bool AllAreActive(){
        for(int i = 0; i < activatedTowers.Length; i++){
            if(!activatedTowers[i]) return false;
        }
        return true;
    }

    public void ActivateTower(int towerNumber){
        activatedTowers[towerNumber] = true;
        if(AllAreActive()){
            MissionObjectiveManager.instance.CompleteObjective(7);
            WinTheGame();
        }
    }

    public void WinTheGame(){
        Debug.Log("YOU WON!");
        enableOnWin.SetActive(true);
    }
}
