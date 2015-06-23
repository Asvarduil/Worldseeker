using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the game's overlord.
// ()()   _
// (**) _//
// (,,)//

public struct IntVector3
{
    #region Variables / Properties

    public int x;
    public int y;
    public int z;

    public int Width { get { return x; } }
    public int Height { get { return y; } }
    public int Depth { get { return z; } }

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
}

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
        StartCoroutine(CreateBiomeCells(dimensions));
    }

    #endregion Hooks

    #region Methods

    private IEnumerator CreateBiomeCells(Vector3 dimensions)
    {
        if (PrototypeBiomeCell == null)
            throw new InvalidOperationException("Please set a prototype Biome Cell, before running the Biome Creation process.");

        // Parse dimensions as integers, for array operations.
        IntVector3 dims = new IntVector3(dimensions);

        _biomeCells = new GameObject[dims.Width, dims.Height, dims.Depth];

        // Generate volume of cells...
        int x = dims.x % 2 == 1 ? dims.x / 2 + 1 : dims.x / 2;
        int y = dims.y % 2 == 1 ? dims.y / 2 + 1 : dims.y / 2;
        int z = 0;

        for (z = 0; z < dimensions.z; z++)
        {
            IntVector3 coreDataPosition = new IntVector3(0, 0, z);
            AddBiomeCell(coreDataPosition);

            yield return null;

            for (y = 1; y < dimensions.y / 2; y++)
            {
                IntVector3 aboveDataPosition = new IntVector3(0, y, z);
                IntVector3 belowDataPosition = new IntVector3(0, -y, z);

                AddBiomeCell(aboveDataPosition);
                AddBiomeCell(belowDataPosition);

                yield return null;
            }

            for (x = 1; x < dimensions.x / 2; x++)
            {
                IntVector3 leftDataPosition = new IntVector3(-x, 0, z);
                IntVector3 rightDataPosition = new IntVector3(x, 0, z);

                AddBiomeCell(leftDataPosition);
                AddBiomeCell(rightDataPosition);

                yield return null;
            }
        }
    }

    /// <summary>
    /// Adds a BiomeCell to the generator, and to this object as a child.  This is necessary
    /// for biome generation.
    /// </summary>
    /// <param name="realPosition">World coordinates to place the cell</param>
    /// <returns>The generated biome cell</returns>
    private void AddBiomeCell(IntVector3 dataPosition)
    {
        Vector3 realPosition = transform.position;
        realPosition.x += dataPosition.x;
        realPosition.y += dataPosition.y;
        realPosition.z += dataPosition.z;

        var cell = (GameObject) GameObject.Instantiate(PrototypeBiomeCell, realPosition, Quaternion.identity);
        cell.transform.parent = transform;
        
        _biomeCells[dataPosition.x,dataPosition.y,dataPosition.z] = cell;
    }

    #endregion Methods
}
