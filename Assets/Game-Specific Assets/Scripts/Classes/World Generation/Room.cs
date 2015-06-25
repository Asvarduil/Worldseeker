using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class Room : ICloneable, IJsonSavable
{
    #region Variables / Properties

    public string Name;
    public int Id;
    public BiomeType BiomeType;
    public Vector3 Position;
    public Vector3 Rotation;
    public List<RoomCell> Cells;

    private List<RoomCell> _roomDoors;
    public List<RoomCell> RoomDoors
    {
        get
        {
            if (_roomDoors == null)
            {
                _roomDoors = new List<RoomCell>();
                for (int i = 0; i < Cells.Count; i++)
                {
                    RoomCell current = Cells[i];
                    if (current.CellType != RoomCellType.Door)
                        continue;

                    _roomDoors.Add(current);
                }
            }

            return _roomDoors;
        }
    }

    #endregion Variables / Properties

    #region Constructor

    public Room()
    {
    }

    #endregion Constructor

    #region Implemented Methods

    public object Clone()
    {
        return new Room
        {
            Name = this.Name,
            Id = this.Id,
            Position = this.Position,
            Rotation = this.Rotation,
            Cells = this.Cells
        };
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Id = state["Id"].AsInt;

        Position = state["Position"].ImportVector3();
        Rotation = state["Rotation"].ImportVector3();
        Cells = state["Cells"].AsArray.UnfoldJsonArray<RoomCell>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Id"] = new JSONData(Id);
        state["Position"] = Position.ExportAsJson();
        state["Rotation"] = Rotation.ExportAsJson();
        state["Cells"] = Cells.FoldList();

        return state;
    }

    #endregion Implemented Methods

    #region Methods

    /// <summary>
    /// Finds the first room cell between two rooms that overlaps.
    /// </summary>
    /// <param name="a">The room to check</param>
    /// <param name="b">The existing room</param>
    /// <returns>True if any cells overlap, otherwise false.</returns>
    public static bool DoRoomsOverlap(Room a, Room b)
    {
        bool overlapExists = false;

        for(int i = 0; i < a.Cells.Count; i++)
        {
            RoomCell current = a.Cells[i];
            Vector3 realPosition = a.Position + current.Position;

            for(int j = 0; j < b.Cells.Count; j++)
            {
                RoomCell suspect = b.Cells[j];
                Vector3 suspectPosition = b.Position + suspect.Position;

                overlapExists = (realPosition == suspectPosition);
                if (overlapExists)
                    break;
            }

            if (overlapExists)
                break;
        }

        return overlapExists;
    }

    /// <summary>
    /// Determines if two rooms share a pair of adjacent "doors".
    /// </summary>
    /// <param name="a">The room to check</param>
    /// <param name="b">The existing room</param>
    /// <returns>True if the two rooms have two adjacent doors, otherwise false.</returns>
    public static bool AreRoomDoorsJoined(Room a, Room b)
    {
        bool roomsJoined = false;

        for (int i = 0; i < a.RoomDoors.Count; i++)
        {
            RoomCell current = a.RoomDoors[i];
            Vector3 currentPosition = a.Position + current.Position;
            Vector3 currentRotation = a.Rotation + current.Rotation;

            for(int j = 0; j < b.RoomDoors.Count; j++)
            {
                RoomCell suspect = b.RoomDoors[j];
                Vector3 suspectPosition = b.Position + suspect.Position;
                Vector3 suspectRotation = b.Rotation + suspect.Rotation;

                // We're now doing floating point math...
                // Get the distance between the two cell positions.
                // If that distance is not 1 world unit apart, the two doors are not adjacent.
                float distance = Vector3.Distance(currentPosition, suspectPosition);
                bool areAdjacent = Mathf.Abs(distance - 1.0f) < 0.001f;

                // Also, check that the two doors are facing each other.
                Vector3 normalized = (currentRotation - suspectRotation).normalized;
                float direction = Vector3.Dot(normalized, Vector3.up);
                bool areFacing = Mathf.Approximately(direction, 1.0f);
                                
                roomsJoined |= areAdjacent && areFacing;

                // Unit Test Debug code...
                string stateMessage = string.Format(
                    "The doors at {0} and {1} {2} adjacent, and {3} facing each other.", 
                    currentPosition, 
                    suspectPosition, 
                    (areAdjacent ? "are" : "aren't"),
                    (areFacing ? "are" : "aren't")
                );
                
                string distanceMessage = string.Format(
                    "The distance between the rooms is {0} units",
                    distance
                );

                string anglesMessage = string.Format(
                    "The normalized magnitude between rotations {0} and {1} is {2}.",
                    currentRotation,
                    suspectRotation,
                    direction
                );

                Console.WriteLine(stateMessage);
                Console.WriteLine(distanceMessage);
                Console.WriteLine(anglesMessage);
            }
        }

        return roomsJoined;
    }

    #endregion Methods
}
