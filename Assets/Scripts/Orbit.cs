using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float speed;
    Vector3 playerOffset;
    // Start is called before the first frame update
    void Start()
    {
        playerOffset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + playerOffset;
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        playerOffset = transform.position - target.position;
    }
}
