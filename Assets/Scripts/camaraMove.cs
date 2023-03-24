using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaraMove : MonoBehaviour
{

    public GameObject player;

    public Vector3 localPos;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + localPos;
    }
}
