using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform tele1;
    public Transform tele2;
    public Transform tele3;
    public Transform tele4;
    public Transform tele5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if(Input.GetKeyUp(KeyCode.F2)){
            controller.enabled = false;
            transform.position = tele1.position;
            controller.enabled = true;
        } else if(Input.GetKeyUp(KeyCode.F3)){
            controller.enabled = false;
            transform.position = tele2.position;
            controller.enabled = true;
        } else if(Input.GetKeyUp(KeyCode.F4)){
            controller.enabled = false;
            transform.position = tele3.position;
            controller.enabled = true;
        } else if(Input.GetKeyUp(KeyCode.F5)){
            controller.enabled = false;
            transform.position = tele4.position;
            controller.enabled = true;
        } else if(Input.GetKeyUp(KeyCode.F6)){
            controller.enabled = false;
            transform.position = tele5.position;
            controller.enabled = true;
        }
    }
}
