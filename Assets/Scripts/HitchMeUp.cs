using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitchMeUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        Debug.Log("This probably will not print");
        Debug.Log("ok in case this prints, the object that triggers is - " + other.gameObject.name);
        if(other.gameObject.name == "TowHitch"){
            Debug.Log("aight bet hitch me up");
            HitchMeUpDaddy(other.transform.parent);
        }
    }

    void HitchMeUpDaddy(Transform rover){
        transform.parent.parent = rover;
        transform.parent.localPosition = new Vector3(0f, 0.12f, -.22f);
        transform.parent.rotation = transform.parent.parent.rotation;
        transform.parent.parent.GetComponent<RoverController>().trailerAttached = true;
    }
}
