﻿using UnityEngine;

public class MonsterBullet : BulletBase {

    private float _CheckTime = 0.0f;

    public override void Start()
    {
        base.Start();
        Damage = 5.0f;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (_Destructing)
            return;

        base.OnCollisionEnter(collision);

        var player = collision.gameObject.GetComponent<PlayerHealth>();

        if (player == null)
            return;

        player.TakeDamage(Damage);
    }

    public override void Update()
    {
        if(!_Destructing && Time.time >= _CheckTime)
        {
            _CheckTime += 1;
            if(IsLost())
            {
                Destruct();
            }
        }
    }
}
