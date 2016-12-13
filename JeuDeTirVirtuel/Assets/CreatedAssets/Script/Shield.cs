using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    [SerializeField]
    private GameObject _Hand;

    void Awake()
    {
        gameObject.SetActive(false);
    }
    
    public void SetShieldVisibility(bool boolean)
    {
        gameObject.SetActive(boolean);
    }
}
