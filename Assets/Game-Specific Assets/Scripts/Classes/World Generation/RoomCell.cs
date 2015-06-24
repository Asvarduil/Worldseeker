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

    public IntVector3 Position;
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
            MeshPath = this.MeshPath,
            Mesh = this.Mesh,
            CellType = this.CellType
        };

        return result;
    }

    public void ImportState(JSONClass state)
    {
        Position.ImportState(state["Position"].AsObject);
        MeshPath = state["MeshPath"];
        CellType = state["CellType"].ToEnum<RoomCellType>();

        // TODO: Load Mesh asset...
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["Position"] = Position.ExportState();
        state["MeshPath"] = new JSONData(MeshPath);
        state["CellType"] = new JSONData(CellType.ToString());

        return state;
    }

    #endregion Methods
}
