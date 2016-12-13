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

    void LookAtTarget();

    void MoveToInitialPosition();


    void Move();

    #endregion

}
