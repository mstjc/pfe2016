using UnityEngine;
using System.Collections;

public class PositionKeeper : MonoBehaviour {

    Vector3 _LocalPos = Vector3.zero;
    Quaternion _LocalRot = Quaternion.identity;

    void Awake()
    {
        _LocalPos = transform.localPosition;
        _LocalRot = transform.localRotation;
    }

    void OnEnable()
    {
        transform.localPosition = _LocalPos;
        transform.localRotation = _LocalRot;
    }

    void OnDisable()
    {
        transform.localPosition = _LocalPos;
        transform.localRotation = _LocalRot;
    }
}
