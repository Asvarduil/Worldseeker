using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the game's overlord.
// ()()   _
// (**) _//
// (,,)//



public class BiomeGenerator : DebuggableBehavior
{
    #region Variables / Properties

    public GameObject PrototypeBiomeCell;

    private RoomDatabase _rooms;
    private List<Room> _biomeRooms;
    private GameObject[,,] _biomeCells;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _rooms = FindObjectOfType<RoomDatabase>();
    }

    public void GenerateBiome(Vector3 dimensions, BiomeType biomeType)
    {
        //_biomeRooms = _rooms.GetRoomsByBiomeType(biomeType);
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
