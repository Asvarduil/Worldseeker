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

    public BiomeType BiomeType;

    private RoomDatabase _rooms;
    private List<Room> _biomeRooms;
    private GameObject[,,] _biomeCells;

    #endregion Variables / Properties

    #region Constructor

    public BiomeGenerator(List<Room> roomSet)
    {
        _biomeRooms = roomSet;
    }

    #endregion Constructor

    #region Hooks

    public void Start()
    {
        _rooms = FindObjectOfType<RoomDatabase>();
    }

    public List<Room> GenerateBiome(int roomCountCutoff, BiomeType biomeType)
    {
        List<Room> biome = new List<Room>();

        // Large room used to ensure that door pairing and overlaps don't occur.
        Room validationRoom = new Room();

        // Add the first 'landing' room to the list.
        Room landingRoom = _biomeRooms[0];
        biome.Add(landingRoom);
        validationRoom.Cells.AddRange(landingRoom.Cells);

        // As long as it's possible to add more rooms, do so.
        while(biome.Count < roomCountCutoff)
        {
            // alternate between adding connectors to each door, and more rooms.
        }

        // Any unpaired doors now need to be sealed with a terminus.
        

        return biome;
    }

    #endregion Hooks

    #region Methods

    #endregion Methods
}
