using System;
using UnityEngine;
using SimpleJSON;

public enum RoomCellType
{
    Space = 0,
    Occupied = 1,
    Door = 2
}

[Serializable]
public class RoomCell : IJsonSavable, ICloneable
{
    #region Variables / Properties

    public Vector3 Position;
    public Vector3 Rotation;
    public string MeshPath;
    public Mesh Mesh;
    public RoomCellType CellType;

    #endregion Variables / Properties

    #region Constructors

    public RoomCell()
    {
    }

    #endregion Constructors

    #region Methods

    public object Clone()
    {
        RoomCell result = new RoomCell
        {
            Position = this.Position,
            Rotation = this.Rotation,
            MeshPath = this.MeshPath,
            Mesh = this.Mesh,
            CellType = this.CellType
        };

        return result;
    }

    public void ImportState(JSONClass state)
    {
        Position = state["Position"].ImportVector3();
        Rotation = state["Rotation"].ImportVector3();
        MeshPath = state["MeshPath"];
        CellType = state["CellType"].ToEnum<RoomCellType>();

        // TODO: Load Mesh asset...
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Position"] = Position.ExportAsJson();
        state["Rotation"] = Rotation.ExportAsJson();
        state["MeshPath"] = new JSONData(MeshPath);
        state["CellType"] = new JSONData(CellType.ToString());

        return state;
    }

    #endregion Methods
}
