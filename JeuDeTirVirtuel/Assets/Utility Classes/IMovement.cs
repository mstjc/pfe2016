using UnityEngine;
using System.Collections;

public interface IMovement {

    #region Properties

    /// <summary>
    /// Animator for the monster
    /// </summary>
    Animator MonsterAnimator
    {
        get;
    }

    /// <summary>
    /// Min distance that the monster can go from the target
    /// </summary>
    float MinDistanceToTarget
    {
        get;
        set;
    }

    /// <summary>
    /// Reference to the monster
    /// </summary>
    GameObject Monster
    {
        get;
        set;
    }

    /// <summary>
    /// Target (direction to move)
    /// </summary>
    GameObject Target
    {
        get;
        set;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Indicates if it can move at the moment
    /// </summary>
    /// <returns>If it can move</returns>
    bool CanMove();

    /// <summary>
    /// Dispose of memory.
    /// </summary>
    void Disable();

    /// <summary>
    /// Enable the moving
    /// </summary>
    void Enable();

    /// <summary>
    /// Initialize all default values
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="target"></param>
    /// <param name="minDistanceToTarget"></param>
    void InitializeValues(GameObject monster, GameObject target, float minDistanceToTarget);

    /// <summary>
    /// Move to the initial position
    /// </summary>
    void MoveToInitialPosition();

    /// <summary>
    /// Move in the direction
    /// </summary>
    /// <param name="delta">Time before last move</param>
    void Move(float delta);

    /// <summary>
    /// Update the animation monster
    /// </summary>
    void UpdateAnimation();

    #endregion

}
