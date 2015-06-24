using System;
using UnityEngine;
using SimpleJSON;

public struct IntVector3 : IJsonSavable, ICloneable
{
    #region Variables / Properties

    public int x;
    public int y;
    public int z;

    /// <summary>
    /// Calculates the magnitude of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public float Magnitude
    {
        get { return Mathf.Sqrt((x ^ 2) + (y ^ 2) + (z ^ 2)); }
    }

    #endregion Variables / Properties

    #region Constructor

    public IntVector3(Vector3 init)
    {
        x = (int)init.x;
        y = (int)init.y;
        z = (int)init.z;
    }

    public IntVector3(int inX, int inY, int inZ)
    {
        x = inX;
        y = inY;
        z = inZ;
    }

    #endregion Constructor

    #region Operators

    public static IntVector3 operator + (IntVector3 lhs, IntVector3 rhs)
    {
        lhs.x += rhs.x;
        lhs.y += rhs.y;
        lhs.z += rhs.z;

        return lhs;
    }

    public static IntVector3 operator - (IntVector3 lhs, IntVector3 rhs)
    {
        lhs.x -= rhs.x;
        lhs.y -= rhs.y;
        lhs.z -= rhs.z;

        return lhs;
    }

    public static bool operator == (IntVector3 lhs, IntVector3 rhs)
    {
        return lhs.x == rhs.x
               && lhs.y == rhs.y
               && lhs.z == rhs.z;
    }

    public static bool operator != (IntVector3 lhs, IntVector3 rhs)
    {
        return lhs.x != rhs.x
               && lhs.y != rhs.y
               && lhs.z != rhs.z;
    }

    #endregion Operators

    #region Implemented Methods

    public object Clone()
    {
        var result = new IntVector3();
        result.x = x;
        result.y = y;
        result.z = z;

        return result;
    }

    public void ImportState(JSONClass state)
    {
        x = state["x"].AsInt;
        y = state["y"].AsInt;
        z = state["z"].AsInt;
    }

    public JSONClass ExportState()
    {
        JSONClass state = new JSONClass();

        state["x"] = new JSONData(x);
        state["y"] = new JSONData(y);
        state["z"] = new JSONData(z);

        return state;
    }

    public override string ToString()
    {
        string result = string.Format("[{0}, {1}, {2}]", x, y, z);
        return result;
    }

    #endregion Implemented Methods

    #region Methods

    public static float Distance(IntVector3 a, IntVector3 b)
    {
        return (a - b).Magnitude;
    }

    #endregion Methods
}
