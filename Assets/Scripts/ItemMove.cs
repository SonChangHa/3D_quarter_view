using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    float localYPos;
    float localTime;
    // Start is called before the first frame update
    void Start()
    {
        localYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        localTime += Time.deltaTime;
        float yPos = localYPos + Mathf.Sin(localTime);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate(0, 30 * Time.deltaTime, 0);

    }
}
