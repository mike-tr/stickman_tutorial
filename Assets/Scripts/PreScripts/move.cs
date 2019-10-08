using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public Rigidbody2D rig;
    public Vector2 offset;
    public bool drawOffset = true;

    public float force_multiplier = 10;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            Vector2 force = (rig.position + offset) - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mag = Mathf.Clamp(force.magnitude, 0, 1.2f);
            force = force.normalized * mag;
            rig.AddForce(-force * Time.deltaTime * 1000 * force_multiplier);
        }
    }
}
