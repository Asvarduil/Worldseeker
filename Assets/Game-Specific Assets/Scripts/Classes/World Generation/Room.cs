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
    public GameObject Prefab;
    public Vector3 Dimensions;
    public Vector3 Position;

    public List<RoomDoor> RoomDoors;

    #endregion Variables / Properties

    #region Constructor

    public Room()
    {
    }

    #endregion Constructor

    #region Methods

    public object Clone()
    {
        return new Room
        {
            Name = this.Name,
            Id = this.Id,
            BiomeType = this.BiomeType,
            Prefab = this.Prefab,
            Dimensions = this.Dimensions,
            Position = this.Position,
            RoomDoors = this.RoomDoors
        };
    }

    public void ImportState(JSONClass state)
    {
        Name = state["Name"];
        Id = state["Id"].AsInt;
        BiomeType = (BiomeType) Enum.Parse(typeof(BiomeType), state["BiomeType"]);

        string prefabPath = state["Prefab"];

        Vector3 dimensions = new Vector3();
        dimensions.x = state["width"].AsFloat;
        dimensions.y = state["height"].AsFloat;
        dimensions.z = state["depth"].AsFloat;
        Dimensions = dimensions;

        Vector3 position = new Vector3();
        position.x = state["x"].AsFloat;
        position.y = state["y"].AsFloat;
        position.z = state["z"].AsFloat;
        Position = position;

        RoomDoors = state["RoomDoors"].AsArray.UnfoldJsonArray<RoomDoor>();
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Name"] = new JSONData(Name);
        state["Id"] = new JSONData(Id);
        state["BiomeType"] = new JSONData(BiomeType.ToString());

        state["RoomDoors"] = RoomDoors.FoldList();

        return state;
    }

    #endregion Methods
}
