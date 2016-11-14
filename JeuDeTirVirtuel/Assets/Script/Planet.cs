using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    [SerializeField]
    private Vector3 _Direction = Vector3.right;

    [SerializeField]
    private Transform _RotateAround;

    [SerializeField]
    private float _Speed = 10F;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.RotateAround(_RotateAround ? _RotateAround.transform.position : Vector3.zero, _Direction, _Speed * Time.deltaTime);
        transform.LookAt(Vector3.zero);
	}
}
