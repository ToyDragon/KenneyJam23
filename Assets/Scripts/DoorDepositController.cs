using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDepositController : MonoBehaviour
{
    public bool paid = false;
    public GameObject hintObj;
    public TMPro.TMP_Text hintText;
    public Transform doorLeft;
    public Transform doorRight;
    public Vector3 doorLeftOPos;
    public Vector3 doorRightOPos;
    public float payTime = 0;
    void Update()
    {
        if (paid) {
            doorLeft.transform.position = doorLeftOPos + Mathf.Min((Time.time - payTime) * .25f, 1f) * Vector3.left;
            doorRight.transform.position = doorRightOPos + Mathf.Min((Time.time - payTime) * .25f, 1f) * Vector3.right;
            return;
        }
        hintText.SetText($"{Crystal.instance.count} / 3\nmarsium");
        if (Crystal.instance.count >= 3) {
            bool inRange = (RoverController.instance.transform.position - transform.position).magnitude < 3;
            if (inRange && Input.GetKeyDown(KeyCode.Space)) {
                Crystal.instance.TryRemove();
                Crystal.instance.TryRemove();
                Crystal.instance.TryRemove();
                hintObj.SetActive(false);
                paid = true;
                payTime = Time.time;
                doorLeftOPos = doorLeft.position;
                doorRightOPos = doorRight.position;
            }
        }
    }
}
