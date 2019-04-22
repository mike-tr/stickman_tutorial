using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    right = 0,
    left = 1,
}

public partial class BasicStickMan : MonoBehaviour
{
    private Dictionary<Rigidbody2D, _Muscle> musclesByRg = new Dictionary<Rigidbody2D, _Muscle>();
    public _Muscle[] muscles;

    [Space]
    public Rigidbody2D _legbr;
    public Rigidbody2D _legbl;
    public Rigidbody2D _legur;
    public Rigidbody2D _legul;
    public Rigidbody2D _hip;
    ///added
    [Space]
    public Rigidbody2D _handr;
    public Rigidbody2D _handl;

    public float movement_speed;

    _RaycastDoll dollRaycast;

    private _Muscle[] upper_leg = new _Muscle[2];
    private _Muscle[] lower_leg = new _Muscle[2];
    private _Muscle hip;

    //added
    public _Muscle[] hand = new _Muscle[2];

    Vector2 targetPos;

    private void Start() {
        dollRaycast = new _RaycastDoll(transform);

        foreach (_Muscle muscle in muscles) {
            musclesByRg.Add(muscle.bone, muscle);
        }

        lower_leg[(int)Direction.right] = musclesByRg[_legbr];
        lower_leg[(int)Direction.left] = musclesByRg[_legbl];
        upper_leg[(int)Direction.right] = musclesByRg[_legur];
        upper_leg[(int)Direction.left] = musclesByRg[_legul];
        hip = musclesByRg[_hip];
        //added
        hand[(int)Direction.left] = musclesByRg[_handl];
        hand[(int)Direction.right] = musclesByRg[_handr];
    }

    bool walk = false;
    private void Update() {
        if (Input.GetKey(KeyCode.Space)) {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            walk = true;
        } else if (walk) {
            walk = false;
            foreach (_Muscle muscle in muscles)
                muscle.bone.velocity *= 0.1f;
        }

        if (Input.GetKey(KeyCode.Q)) {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (AttackState[0] == 0)
                AttackState[0] = 1;
        } else if (AttackState[0] != 0) {
            AttackState[0] = 0;
        }

        if (Input.GetKey(KeyCode.E)) {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (AttackState[1] == 0)
                AttackState[1] = 1;
        } else if (AttackState[1] != 0) {
            AttackState[1] = 0;
        }
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (AttackState[0] > 0) {
            Attack(0);
        }
        if(AttackState[1] > 0) {
            Attack(1);
        }

        if (walk) {
            Walk();
        }

        foreach (_Muscle muscle in muscles) {
            muscle.ActivateMuscle();
        }
    }

    int walk_direction;
    float CurrentCycleTime = 0;
    float time_passed = 0;
    int state = 0;

    public float timeCycle = 0.25f;
    public void Walk() {
        //RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.left * 2, Vector2.down);

        RaycastHit2D hit = dollRaycast.RaycastClosest(transform.position, Vector2.down);

        Vector2 dir = (targetPos - (Vector2)transform.position);
        walk_direction = dir.x < 0 ? -1 : 1;

        WalkCycle(walk_direction);
        
        Vector2 walkVector = (walk_direction * hit.normal) * movement_speed;
        walkVector = new Vector2(walkVector.y, -walkVector.x + .1f * movement_speed);

        //hip.AddForce(dd , ForceMode2D.Impulse, true);
        hip.OverloadMuscleRot(6);
        hip.AddMovement(walkVector);
        //hip.AddMovement(Vector2.right * walk_direction * movement_speed);
    }

    public void WalkCycle(int dir) {
        int fl = (int)Direction.right;
        int sl = (int)Direction.left;
        if (state > 1) {
            fl = (int)Direction.left;
            sl = (int)Direction.right;
        }
        switch (state) {
            case 2:
            case 0:
                CurrentCycleTime = timeCycle;
                upper_leg[fl].SetMuscleRot(90 * dir);
                upper_leg[sl].SetMuscleRot(0);
                //foot
                lower_leg[fl].SetMuscleRot(0);
                lower_leg[sl].SetMuscleRot(-25 * dir);
                break;
            case 3:
            case 1:
                CurrentCycleTime = timeCycle * 0.33f;
                upper_leg[fl].SetMuscleRot(0);
                upper_leg[sl].SetMuscleRot(45 * dir);
                //foot
                lower_leg[fl].SetMuscleRot(0);
                lower_leg[sl].SetMuscleRot(-25 * dir);
                break;
        }

        time_passed += Time.fixedDeltaTime;
        if (time_passed > CurrentCycleTime) {
            time_passed = 0;
            state++;
            if (state > 3)
                state = 0;
        }
    }
}

public class _RaycastDoll
{
    private List<Collider2D> transform_colliders;

    public _RaycastDoll(Transform parent) {
        transform_colliders = new List<Collider2D>();
        foreach (Collider2D collider in parent.GetComponentsInChildren<Collider2D>()) {
            //Debug.Log(collider.name);
            transform_colliders.Add(collider);
        }
    }

    public RaycastHit2D RaycastClosest(Vector2 origin, Vector2 dir, float distance = 5f) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, distance);
        float closest_distance = float.MaxValue;
        RaycastHit2D closest_hit = new RaycastHit2D();
        foreach (RaycastHit2D hit in hits) {
            if (!IsChild(hit.collider)) {
                if (hit.distance < closest_distance) {
                    closest_distance = hit.distance;
                    closest_hit = hit;
                }
            }
        }
        return closest_hit;
    }

    public bool IsChild(Collider2D col) {
        foreach (Collider2D collider in transform_colliders) {
            if (collider == col)
                return true;
        }
        return false;
    }
}