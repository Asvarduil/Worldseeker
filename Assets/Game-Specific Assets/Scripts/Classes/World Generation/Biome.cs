using System;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeType
{
    Overworld
}

[Serializable]
public class Biome
{
    #region Variables / Properties

    public string Name;
    public BiomeType Type;
    public int RoomCountCutoff;

    public List<Room> Rooms;
    public List<Door> Doors;

    public Room DefaultRoom;

    #endregion Variables / Properties

    #region Constructor

    public Biome()
    {
    }

    #endregion Constructor

    #region Methods

    #endregion Methods
}
