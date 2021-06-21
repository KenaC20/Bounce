using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform TargetObject;
    public Vector3 offset = Vector3.one;
    public Vector3 angle = Vector3.zero;
    public bool smoothedFollow = false;
    public float smoothSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.Euler(angle);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos;
        newPos = TargetObject.position + offset;

        if (!smoothedFollow)
        {
            transform.position = newPos;
        }
        else
        {
            transform.position += (newPos - transform.position) * Time.deltaTime * smoothSpeed;
        }
    }
}
