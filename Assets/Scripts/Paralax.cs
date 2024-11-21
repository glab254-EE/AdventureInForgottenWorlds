using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    Vector3 startPosition;

    public Camera cam;
    public float modifier;

    float width;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        cam = Camera.main;
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 camOffset = cam.transform.position;
        float difference = cam.transform.position.y - startPosition.y;
        transform.position = startPosition + new Vector3(camOffset.x* modifier, difference, 0);

        if ((cam.transform.position.x - transform.position.x) > width*1.5-cam.orthographicSize*2)
        {
            startPosition += new Vector3(width*2, 0, 0);
        }

        if ((cam.transform.position.x - transform.position.x) < -(width*1.5-cam.orthographicSize*2))
        {
            startPosition -= new Vector3(width*2, 0, 0);
        }
    }
}
