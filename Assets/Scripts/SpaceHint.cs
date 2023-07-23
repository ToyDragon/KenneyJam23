using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceHint : MonoBehaviour
{
    public static SpaceHint instance;
    void OnEnable() {
        instance = this;
    }
    public void Set(Vector3 pos) {
        transform.position = pos;
    }
    public void Set(Transform trans) {
        transform.position = trans.position;
    }
    public void Reset() {
        transform.position = Vector3.down * 100f;
    }
}
