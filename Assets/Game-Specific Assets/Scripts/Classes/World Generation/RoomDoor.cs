using System;
using UnityEngine;
using SimpleJSON;

[Serializable]
public class RoomDoor : ICloneable, IJsonSavable
{
    #region Variables / Properties

    public int Id;
    public Vector3 Position;

    #endregion Variables / Properties

    #region Constructor

    public RoomDoor()
    {
    }

    #endregion Constructor

    #region Methods

    public object Clone()
    {
        return new RoomDoor
        {
            Id = this.Id,
            Position = this.Position
        };
    }

    public void ImportState(JSONClass state)
    {
        Id = state["Id"].AsInt;

        Vector3 position = new Vector3();
        position.x = state["x"].AsFloat;
        position.y = state["y"].AsFloat;
        position.z = state["z"].AsFloat;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Id"] = new JSONData(Id);
        state["x"] = new JSONData(Position.x);
        state["y"] = new JSONData(Position.y);
        state["z"] = new JSONData(Position.z);

        return state;
    }

    #endregion Methods
}
