using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour {

    bool _Destruct = false;
    float _TimeLeft;

    void OnCollisionEnter(Collision collision)
    {
        StartAutoDestruction();
        GetComponent<Rigidbody>().useGravity = true;
    }

    void Update()
    {
        if(_Destruct)
        {
            _TimeLeft -= Time.deltaTime;

            if(_TimeLeft <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void StartAutoDestruction()
    {
        var light = GetComponent<Light>();
        if (light != null)
            light.intensity = 0.0f;

        _Destruct = true;
        _TimeLeft = 2.0f;
    }

    private bool IsLostBullet()
    {
        return Mathf.Abs(transform.position.x) > 50.0f || Mathf.Abs(transform.position.z) > 50.0f;
    }
}
