using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    string targetText = "";
    float writeSpeed = 0.05f;
    float currentTime = 0f;
    bool writing = false;
    public AudioSource morseCode;

    public static ChatManager instance;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable(){
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(writing){
            currentTime += Time.deltaTime;
            int numChars = (int)(currentTime/writeSpeed);

            if(Input.GetKeyUp(KeyCode.Return)){
                numChars = targetText.Length + 1;
                GetComponentInChildren<TextMeshProUGUI>().text = targetText;
            }

            if(numChars > targetText.Length){
                writing = false;
                morseCode.Stop();
            } else {
                GetComponentInChildren<TextMeshProUGUI>().text = targetText.Substring(0,numChars);
            }
        } else {
            if(Input.GetKeyUp(KeyCode.Return)){
                Hide();
            }
        }
    }

    public void ShowAndStartText(string targetText){
        currentTime = 0f;
        this.targetText = targetText;
        GetComponentInChildren<TextMeshProUGUI>().text = "";
        writing = true;
        gameObject.SetActive(true);
        morseCode.Play();
    }

    void Hide(){
        gameObject.SetActive(false);
    }
}
