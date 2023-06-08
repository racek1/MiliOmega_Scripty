using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToTarget : MonoBehaviour
{
    public string target;
    private Transform targetObj;
    private void Start()
    {
        targetObj = GameObject.Find(target).transform;
    }
    void Update()
    {
        transform.position = targetObj.transform.position;
    }
}
