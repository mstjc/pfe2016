using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

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
        //var rot = new Vector3(_Palm.transform.eulerAngles.x, _Palm.transform.eulerAngles.y + 90, _Palm.transform.eulerAngles.z);
        //gameObject.transform.eulerAngles = rot;//new Vector3(0, 90, 0);
        //gameObject.transform.position = _Palm.transform.position;
	}
}
