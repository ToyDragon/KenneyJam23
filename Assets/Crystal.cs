using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public static GameObject crystalObject;
    public static bool isCrystalInTrailer = false;
    // Start is called before the first frame update
    void Start()
    {
        crystalObject = gameObject;
        crystalObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Toggle(){
        crystalObject.SetActive(!crystalObject.activeSelf);
        isCrystalInTrailer = !isCrystalInTrailer;
    }
}
