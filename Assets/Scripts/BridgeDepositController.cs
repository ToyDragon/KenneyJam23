using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeDepositController : MonoBehaviour
{
    public bool paid = false;
    public GameObject hintObj;
    public TMPro.TMP_Text hintText;
    void Update()
    {
        if (paid) {
            return;
        }
        if (Crystal.instance.count < 3) {
            hintText.SetText($"{Crystal.instance.count} / 3\nmarsium");
        } else {
            bool inRange = (RoverController.instance.transform.position - transform.position).magnitude < 3;
            if (inRange && Input.GetKeyDown(KeyCode.Space)) {
                Crystal.instance.TryRemove();
                Crystal.instance.TryRemove();
                Crystal.instance.TryRemove();
                hintObj.SetActive(false);
                paid = true;
            }
        }
    }
}
