using UnityEngine;
using System.Collections.Generic;

public class KillableEntity : DebuggableBehavior
{
    #region Variables / Properties

    public HealthSystem Health;
    public List<GameEvent> DeathEvents;

    private GameEventController _events;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _events = GameEventController.Instance;
        Health.OnDamageTaken += RunDeathEvents;
    }

    #endregion Hooks

    #region Methods

    private void RunDeathEvents()
    {
        if (Health.IsDead)
            return;

        StartCoroutine(_events.ExecuteGameEventGroup(DeathEvents));
    }

    #endregion Methods
}
