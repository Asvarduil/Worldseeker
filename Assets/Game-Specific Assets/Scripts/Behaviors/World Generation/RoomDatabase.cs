using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RoomDatabase : JsonBlobLoaderBase
{
    #region Variables / Properties

    public List<Room> Rooms;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        MapBlob();
    }

    #endregion Hooks

    #region Methods

    public List<Room> GetRoomsByBiomeType(BiomeType biomeType)
    {
        var results = Rooms.Where(r => r.BiomeType == biomeType).ToList();
        return results;
    }

    private void MapBlob()
    {
        // TODO: Implement
    }

    #endregion Methods
}
