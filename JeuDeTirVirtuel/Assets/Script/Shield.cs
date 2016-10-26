using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

    [SerializeField]
    private GameObject _Palm;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.position = _Palm.transform.position;
    }
}
