using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeDepositController : MonoBehaviour
{
    public bool paid = false;
    public GameObject hintObj;
    public TMPro.TMP_Text hintText;
    public Transform bridgeFarA;
    public Transform bridgeFarB;
    public Transform bridgeNearA;
    public Transform bridgeNearB;
    void Update()
    {
        if (paid) {
            bridgeFarA.transform.localEulerAngles -= new Vector3(bridgeFarA.transform.localEulerAngles.x * Mathf.Clamp01(Time.deltaTime * 15), 0, 0);
            bridgeFarB.transform.localEulerAngles -= new Vector3(bridgeFarB.transform.localEulerAngles.x * Mathf.Clamp01(Time.deltaTime * 15), 0, 0);
            bridgeNearA.transform.localEulerAngles -= new Vector3(0, 0, bridgeNearA.transform.localEulerAngles.z * Mathf.Clamp01(Time.deltaTime * 15));
            bridgeNearB.transform.localEulerAngles -= new Vector3(0, 0, bridgeNearB.transform.localEulerAngles.z * Mathf.Clamp01(Time.deltaTime * 15));
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
                MissionObjectiveManager.instance.CompleteObjective(3);
            }
        }
    }
}
