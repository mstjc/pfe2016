using UnityEngine;
using System;

public class MonsterSoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource _BornSource;

    [SerializeField]
    private AudioSource _HitSource;

    [SerializeField]
    private AudioSource _AttackSource;

    [SerializeField]
    private AudioSource _DieSource;

    private MonsterManager _MonsterManager;

    void OnEnable()
    {
        SubscribeMonster();
    }

    void OnDisable()
    {
        UnsubscribeMonster();
    }

    private void SubscribeMonster()
    {
        _MonsterManager = GetComponentInParent<MonsterManager>();
        if (_MonsterManager != null)
        {
            _MonsterManager.Born += OnBorn;
            _MonsterManager.Hit += OnHit;
            _MonsterManager.Attack += OnAttack;
            _MonsterManager.Died += OnDied;
        }
    }

    private void UnsubscribeMonster()
    {
        if (_MonsterManager != null)
        {
            _MonsterManager.Born -= OnBorn;
            _MonsterManager.Hit -= OnHit;
            _MonsterManager.Died -= OnDied;
        }
    }

    private void OnBorn(object sender, EventArgs e)
    {
        if(_BornSource != null)
        {
            _BornSource.Play();
        }
    }

    private void OnHit(object sender, EventArgs e)
    {
        if (_HitSource != null && !_HitSource.isPlaying && !_AttackSource.isPlaying)
        {
            _HitSource.Play();
        }
    }

    private void OnAttack(object sender, EventArgs e)
    {
        if (_AttackSource != null)
        {
            _AttackSource.Play();
        }
    }

    private void OnDied(object sender, EventArgs e)
    {
        if(_DieSource != null)
        {
            _DieSource.Play();
        }
    }
}
