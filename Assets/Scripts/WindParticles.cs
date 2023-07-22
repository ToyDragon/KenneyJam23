using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticles : MonoBehaviour
{
    private Vector3 offset;
    void OnEnable() {
        offset = transform.position - Camera.main.transform.position;
    }
    void Update()
    {
        transform.position = Camera.main.transform.position + offset;
    }
}
