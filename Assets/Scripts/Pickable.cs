using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public static List<Pickable> items = new List<Pickable>();

    public Item item;
    void Start() {
        items.Add(this);  
    }

    public void Destroy() {
        items.Remove(this);
        Destroy(gameObject);
    }
}
