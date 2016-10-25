using UnityEngine;

public class StraightFastMovement : StraightMovement {

    public override void InitializeValues(GameObject monster, GameObject target, float minDistanceToTarget)
    {
        Monster = monster;
        Target = target;
        MinDistanceToTarget = minDistanceToTarget;
        MinMovingFreq = 4.0f;
        MaxMovingFreq = 6.0f;
        MaxSpeed = 4.0f;
    }
}
