using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static Controls instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
