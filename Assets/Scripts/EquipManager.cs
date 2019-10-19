using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private Dictionary<BodyPartType, BodyPart> bodyParts = new Dictionary<BodyPartType, BodyPart>();
    public List<Item> inventory = new List<Item>();

    public Equipable equip_item;

    public float strength = 10;
    public float magic = 10;

    public float pickRadius = 10;
    // Start is called before the first frame update
    void Start()
    {
        var parts = transform.GetComponentsInChildren<BodyPart>();
        foreach(var part in parts) {
            bodyParts.Add(part.type, part);
        }

        EquipItem(equip_item);
    }

    void Pickup(Pickable pick) {
        if (!pick)
            return;
        var item = pick.item;
        if (item) {
            if(item.GetType() == typeof(Equipable)) {
                EquipItem((Equipable)item);
            } else {
                inventory.Add(item);
            }
        }
        pick.Destroy();
    }

    void EquipItem(Equipable item) {
        if (!item)
            return;
        if(bodyParts.TryGetValue(item.type, out var part)) {
            part.Equip(item);
            strength += item.strength;
            magic += item.magic;
        }
        // equip the item
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Pickable pick = null;
            foreach(Pickable item in Pickable.items) {
                if(Vector2.Distance(transform.position, item.transform.position) < pickRadius) {
                    pick = item;
                    break;
                }
            }
            Pickup(pick);
            // pickup items!
        }
    }
}
