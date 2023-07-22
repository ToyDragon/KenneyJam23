using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)){
            GameObject canvasObject = gameObject.GetComponentInChildren<Canvas>(true).gameObject;
            canvasObject.SetActive(!active);
            active = !active;
        }        
    }
}
