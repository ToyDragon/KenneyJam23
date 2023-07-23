using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public static Crystal instance;
    public List<GameObject> crystals;
    public int count = 0;
    void OnEnable()
    {
        instance = this;
        foreach (var obj in crystals) {
            obj.SetActive(false);
        }
    }
    public void Add(){
        count = Mathf.Min(count + 1, 3);
        crystals[count - 1].SetActive(true);
    }
    public bool TryRemove(){
        if (count <= 0) {
            return false;
        }
        crystals[count - 1].SetActive(false);
        count--;
        return true;
    }
}
