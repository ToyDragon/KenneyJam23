using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitchMeUp : MonoBehaviour
{
    public AudioSource hitchSound;
    // Start is called before the first frame update
    public RoverController roverController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roverController) {
            transform.parent.position = roverController.model.position - roverController.model.forward * .22f;
            var backPos = transform.position - transform.forward * .5f;
            Debug.DrawRay(backPos + Vector3.up*.5f, Vector3.down, Color.blue, .01f);
            if (Physics.Raycast(backPos + Vector3.up*.5f, Vector3.down, out var hit, 1f, Physics.AllLayers)) {
                Debug.DrawRay(hit.point, -Vector3.forward, Color.red, .01f);
                var delta = (transform.position - Vector3.up*.12f - hit.point).normalized;
                float angleDegrees = Mathf.Asin(-delta.y) * 180 / Mathf.PI;
                transform.parent.localEulerAngles = new Vector3(angleDegrees, -.5f * roverController.animatedWheelDir, 0);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if (roverController) {
            return;
        }
        Debug.Log("This probably will not print");
        Debug.Log("ok in case this prints, the object that triggers is - " + other.gameObject.name);
        if(other.gameObject.name == "TowHitch") {
            Debug.Log("aight bet hitch me up");
            HitchMeUpDaddy(other.transform.parent);
        }
    }

    void HitchMeUpDaddy(Transform rover){
        roverController = rover.GetComponent<RoverController>();
        transform.parent.parent = rover;
        transform.parent.localPosition = new Vector3(0f, 0f, -.22f);
        transform.parent.rotation = transform.parent.parent.rotation;
        transform.parent.parent.GetComponent<RoverController>().trailerAttached = true;
        hitchSound.Play();
    }
}
