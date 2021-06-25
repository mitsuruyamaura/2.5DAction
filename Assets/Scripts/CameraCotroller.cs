using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCotroller : MonoBehaviour
{
    [SerializeField]
    private Transform targetTran;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - targetTran.position;
    }

    void Update()
    {
        if (targetTran != null) {
            transform.position = targetTran.position + offset;
        }
    }
}
