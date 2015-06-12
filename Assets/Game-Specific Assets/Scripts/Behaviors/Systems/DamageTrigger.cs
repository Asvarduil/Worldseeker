using UnityEngine;
using System.Collections.Generic;

public class DamageTrigger : DebuggableBehavior
{
    #region Variables / Properties

    public List<string> AffectedTags;

    public int ContactDamage;
    public Lockout PeriodicDamageLockout;

    public List<GameEvent> EnterEvents;
    public List<GameEvent> ExitEvents;

    private GameEventController _events;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _events = GameEventController.Instance;
    }

    public void OnTriggerEnter(Collider who)
    {
        if (!AffectedTags.Contains(who.tag))
            return;

        DealDamage(who.gameObject);
        StartCoroutine(_events.ExecuteGameEventGroup(EnterEvents));
    }

    public void OnTriggerStay(Collider who)
    {
        // If not periodic, don't even bother.
        if (Mathf.Abs(PeriodicDamageLockout.LockoutRate - 0.0f) <= 0.001f)
            return;

        if (!AffectedTags.Contains(who.tag))
            return;

        if (!PeriodicDamageLockout.CanAttempt())
            return;

        DealDamage(who.gameObject);
        PeriodicDamageLockout.NoteLastOccurrence();
    }

    public void OnTriggerExit(Collider who)
    {
        if (!AffectedTags.Contains(who.tag))
            return;

        StartCoroutine(_events.ExecuteGameEventGroup(ExitEvents));
    }

    #endregion Hooks

    #region Methods

    private void DealDamage(GameObject entity)
    {
        KillableEntity target = entity.GetComponent<KillableEntity>();
        if (target == null)
            return;

        target.Health.TakeDamage(ContactDamage);
    }

    #endregion Methods
}
