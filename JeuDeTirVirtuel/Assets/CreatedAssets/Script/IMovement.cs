using UnityEngine;
using System.Collections;

public interface IMovement {

    #region Properties

    bool CanMove
    {
        get;
        set;
    }

    /// <summary>
    /// Animator for the monster
    /// </summary>
    Animator MonsterAnimator
    {
        get;
    }

    /// <summary>
    /// Reference to the monster
    /// </summary>
    GameObject Monster
    {
        get;
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
    /// Look at the target
    /// </summary>
    void LookAtTarget();

    /// <summary>
    /// Move to the initial position
    /// </summary>
    void MoveToInitialPosition();

    /// <summary>
    /// Move in the direction
    /// </summary>
    void Move();

    #endregion

}
