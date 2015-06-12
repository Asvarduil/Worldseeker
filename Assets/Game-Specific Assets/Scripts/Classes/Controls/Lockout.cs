using System;
using UnityEngine;

[Serializable]
public class Lockout
{
    #region Variables / Properties

    public float LockoutRate = 1.0f;
    public float LastAttempt = 0.0f;

    #endregion Variables / Properties

    #region Hooks

    public bool CanAttempt()
    {
        return Time.time > LastAttempt + LockoutRate;
    }

    public void NoteLastOccurrence()
    {
        LastAttempt = Time.time;
    }

    #endregion Hooks
}
