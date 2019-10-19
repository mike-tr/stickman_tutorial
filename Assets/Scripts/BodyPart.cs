using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPartType {
    body,
    hand_l,
    hand_r,
}

public class BodyPart : MonoBehaviour
{
    public BodyPartType type;
    private Equipable item;

    public void Equip(Equipable item) {
        Debug.Log("equiping item - " + item.name);
        if (this.item) {
            Destroy(item);
        }

        item = Instantiate(item);
        item.transform.SetParent(transform);
        item.transform.localPosition = item.equip_position;
        item.transform.localEulerAngles = Vector2.zero;

        // equip the item
    }
}
