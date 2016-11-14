﻿using UnityEngine;
using Leap.Unity;
using System.Collections;

public class DetectorEnablingScript : MonoBehaviour {

    [SerializeField]
    private Detector[] _Detector;
	
    public void EnableDetectors()
    {
        foreach (Detector d in _Detector)
        {
            d.Activate();
        }
    }

    public void DisableDetectors()
    {
        foreach (Detector d in _Detector)
        {
            d.Deactivate();
        }
    }
}
