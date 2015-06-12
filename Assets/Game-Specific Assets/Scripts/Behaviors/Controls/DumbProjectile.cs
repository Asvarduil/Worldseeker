using System;
using System.Collections.Generic;
using UnityEngine;

public class DumbProjectile : DebuggableBehavior
{
    #region Variables / Properties

    public Vector3 Velocity;
    public float Lifetime = 1.0f;
    public int DamageOnHit;
    public List<GameObject> SecondaryEffects;

    public List<GameEvent> ProjectileHitEvents;

    private float _bornAt = 0.0f;
    private GameEventController _events;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _bornAt = Time.time;
        _events = GameEventController.Instance;
    }

    public void FixedUpdate()
    {
        transform.Translate(Velocity);

        if (Time.time > _bornAt + Lifetime)
            Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        OnProjectileHit();

        KillableEntity damage = collision.collider.gameObject.GetComponent<KillableEntity>();
        if(damage != null)
            damage.Health.TakeDamage(DamageOnHit);

        if (!SecondaryEffects.IsNullOrEmpty())
            GenerateSecondaryEffects(collision);

        Destroy(gameObject);
    }

    #endregion Hooks

    #region Methods

    private void GenerateSecondaryEffects(Collision collision)
    {
        Vector3 spawnAt = collision.contacts[0].point;
        Quaternion spawnRotation = Quaternion.Euler(collision.contacts[0].normal);

        for(int i = 0; i < SecondaryEffects.Count; i++)
        {
            GameObject effect = SecondaryEffects[i];    
            Instantiate(effect, spawnAt, spawnRotation);
        }
    }

    private void OnProjectileHit()
    {
        if (_events == null)
            return;

        StartCoroutine(_events.ExecuteGameEventGroup(ProjectileHitEvents));
    }

    #endregion Methods
}
