using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class _Muscle
{
    public Rigidbody2D bone;
    public Transform root;
    public float restRotation;
    public float force;

    private float addRotation = 0;
    private float currentforce;
    private float forcePercentage;

    public bool disabled = false;


    public void ActivateMuscle() {
        if (disabled)
            return;
        RotateSmooth(restRotation + addRotation, currentforce * forcePercentage);

        addRotation = 0;
        forcePercentage = 1f;
        currentforce = force;
    }

    public void SetMuscleRot(float rot) {
        addRotation = rot;
    }

    public void OverloadMuscleRotStrict(float force) {
        currentforce = force;
    }

    public void AddMovement(Vector2 movement) {
        bone.MovePosition(bone.position + movement * Time.fixedDeltaTime);
    }

    private void RotateSmooth(float rotation, float force) {
        float angle = Mathf.DeltaAngle(bone.rotation, rotation);
        float ratio = angle / 180;
        ratio *= ratio;

        bone.MoveRotation(Mathf.LerpAngle(bone.rotation, rotation, force * ratio * Time.fixedDeltaTime));
        bone.AddTorque(angle * force * (1 - ratio) * .1f);
    }

    //NEW ---- 
    public void AddForce(Vector2 force) {
        bone.AddForce(force);
    }

    public void OverloadMuscleRot(float angle, float addForcePecentage) {
        addRotation = angle;
        forcePercentage *= addForcePecentage;
    }

    public void OverloadMuscleRot(float addForcePecentage) {
        forcePercentage *= addForcePecentage;
    }

    public Vector2 GetRootDirection(Vector2 targetPos) {
        return (targetPos - (Vector2)root.position);
    }
}
