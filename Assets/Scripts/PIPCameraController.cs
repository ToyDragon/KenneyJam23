using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIPCameraController : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(RoverController.instance.transform);
    }
}
