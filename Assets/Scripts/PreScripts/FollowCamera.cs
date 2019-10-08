using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public Vector2 offset = Vector2.up * 5;
    public float speed = 2;
    public float size = 20;
    void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.orthographicSize = size;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = target.position - transform.position;
        dir += offset;
        transform.position += (Vector3)dir * speed * Time.deltaTime;
    }
}
