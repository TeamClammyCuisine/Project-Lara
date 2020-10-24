using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followTransform;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(followTransform.transform.position.x, followTransform.transform.position.y, this.transform.position.z);
    }
}
