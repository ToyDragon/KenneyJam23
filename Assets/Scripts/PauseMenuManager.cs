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
        gameObject.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
        unstuckButton.onClick.AddListener(delegate { Unstuck(); });
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

    void Unstuck(){
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        controller.enabled = true;
    }
}
