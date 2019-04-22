using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicStickMan : MonoBehaviour
{
    public const float AnimStrength = 1.5f;
    
    public float Force = 5000;
    public float range = 2.1f;

    int[] AttackState = new int[2];
    int attack_direction = 0;

    int fl = (int)Direction.right;
    int sl = (int)Direction.left;
    bool[] reversed_dir = new bool[2];

    float time = 0;
    public float switchTime = 0.2f;
    void Attack(int ahand) {
        int _attackingHand = ahand;
        _Muscle _hand = hand[_attackingHand];
        Vector2 dir = (targetPos - (Vector2)transform.position);

        if (AttackState[_attackingHand] == 1) {
            AttackState[_attackingHand] = 2;
            attack_direction = 1;
            fl = (int)Direction.right;
            sl = (int)Direction.left;
            if (dir.x < 0) {
                attack_direction = -1;
                fl = (int)Direction.left;
                sl = (int)Direction.right;
            }
        }

        upper_leg[fl].OverloadMuscleRot(25 * attack_direction, AnimStrength);
        lower_leg[fl].OverloadMuscleRot(25 * attack_direction, AnimStrength);
        lower_leg[fl].AddForce(new Vector2(-5 * attack_direction, -5));

        lower_leg[fl].bone.velocity *= Vector2.zero;

        upper_leg[sl].OverloadMuscleRot(0, AnimStrength);
        lower_leg[sl].OverloadMuscleRot(-25 * attack_direction, AnimStrength);

        lower_leg[sl].bone.velocity *= Vector2.zero;

        hip.bone.velocity *= Vector2.zero;
        hip.OverloadMuscleRot(AnimStrength * 5);

        ////
        Vector2 hand_dir = _hand.GetRootDirection(targetPos);
        if (hand_dir.magnitude > range)
            hand_dir = hand_dir.normalized * range;
        if (reversed_dir[_attackingHand])
        {
            hand_dir.x *= -1f;
        }

        _hand.AddForce(hand_dir * Force);
        time += Time.fixedDeltaTime;
        if(time > switchTime) {
            time = 0;
            reversed_dir[_attackingHand] = !reversed_dir[_attackingHand];
        }
        //Attacks[_attackingHand].Attack(targetPos, AttackForce, AttackForce);
    }
}
