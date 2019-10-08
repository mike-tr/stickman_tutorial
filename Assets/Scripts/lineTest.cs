using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class lineTest : MonoBehaviour
{
    // Start is called before the first frame update

    public LineRenderer line;
    public Vector2 index0 = Vector2.zero;
    public Vector2 index1 = Vector2.one;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, index0);
        line.SetPosition(1, index1);
    }
}
