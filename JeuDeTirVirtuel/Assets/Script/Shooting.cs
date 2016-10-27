using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

    public Rigidbody _Projectile;
    public Transform _ShotPos;
    public float _ShotForce = 1000.0f;
    public float _MoveSpeed = 10.0f;

	void Update () {
	    float h = Input.GetAxis("Horizontal") *Time.deltaTime * _MoveSpeed;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * _MoveSpeed;

        transform.Translate(new Vector3(h, v, 0));

        if(Input.GetButtonUp("Fire1"))
        {
            Rigidbody shot = Instantiate(_Projectile, _ShotPos.position, _ShotPos.rotation) as Rigidbody;
            Debug.Log(_ShotPos.rotation);
            shot.AddForce(shot.transform.forward * _ShotForce);
        }
    }
}
