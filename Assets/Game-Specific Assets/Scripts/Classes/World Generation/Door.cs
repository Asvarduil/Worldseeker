using System;
using UnityEngine;

public enum DoorLockLevel
{
    Red = 0
}

[Serializable]
public class Door
{
    public DoorLockLevel LockLevel;

    public int SourceRoomId;
    public int DestinationRoomId = -1;
}
