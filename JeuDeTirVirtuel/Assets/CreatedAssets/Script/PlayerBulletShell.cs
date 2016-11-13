using UnityEngine;
using System.Collections;

public class PlayerBulletShell : PlayerBullet {

    private bool _HasCollided = false;

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (!_HasCollided && collision.collider.name == "Arena")
        {
            var audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
                _HasCollided = true;
            }
        }
    }
}
