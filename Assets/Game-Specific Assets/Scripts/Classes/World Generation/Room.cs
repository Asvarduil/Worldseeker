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
    public IntVector3 Position;
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
            Cells = this.Cells
        };
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Id = state["Id"].AsInt;

        Position.ImportState(state["Position"].AsObject);
        Cells = state["Cells"].AsArray.UnfoldJsonArray<RoomCell>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Id"] = new JSONData(Id);
        state["Position"] = Position.ExportState();
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
            IntVector3 realPosition = a.Position + current.Position;

            for(int j = 0; j < b.Cells.Count; j++)
            {
                RoomCell suspect = b.Cells[j];
                IntVector3 suspectPosition = b.Position + suspect.Position;

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
            IntVector3 realPosition = a.Position + current.Position;

            for(int j = 0; j < b.RoomDoors.Count; j++)
            {
                RoomCell suspect = b.RoomDoors[j];
                IntVector3 suspectPosition = b.Position + suspect.Position;

                // We're now doing floating point math...
                // Get the distance between the two cell positions.
                // If that distance is not 1 world unit apart, the two doors are not adjacent.
                float distance = IntVector3.Distance(realPosition, suspectPosition);
                bool areAdjacent = Mathf.Abs(distance - 1.0f) < 0.001f;
                roomsJoined |= areAdjacent;

                // Unit Test Debug code...
                string message = string.Format("The doors at {0} and {1} {2} adjacent; the distance is {3}.", 
                    realPosition, 
                    suspectPosition, 
                    (roomsJoined ? "are" : "aren't"),
                    distance);
                Console.WriteLine(message);
            }
        }

        return roomsJoined;
    }

    #endregion Methods
}
