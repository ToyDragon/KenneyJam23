using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public Button unstuckButton;
    public GameObject player;
    bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject canvasObject = gameObject.GetComponentInChildren<Canvas>(true).gameObject;
        //assumes that the pause menu is the first child of the canvas
        canvasObject.transform.GetChild(0).gameObject.SetActive(false);
        unstuckButton.onClick.AddListener(delegate { Unstuck(); });
    }

    // Update is called once per frame
    void Update()
    {
        GameObject canvasObject = gameObject.GetComponentInChildren<Canvas>(true).gameObject;
        if(Input.GetKeyUp(KeyCode.Escape)){
            //assumes that the pause menu is the first child of the canvas
            canvasObject.transform.GetChild(0).gameObject.SetActive(!active);
            active = !active;
        } else if(Input.GetKeyUp(KeyCode.P)){
            GetComponentInChildren<ChatManager>(true).ShowAndStartText("This message should be written out over a few seconds \n \nDo new lines work?\n\n\nmaking this extra long to test the skipping functionality lol idk what to type here matt and chandler are binks im a ui god please work on first try gg ez no re");
        } 
    }

    void Unstuck(){
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        controller.enabled = true;
    }
}
