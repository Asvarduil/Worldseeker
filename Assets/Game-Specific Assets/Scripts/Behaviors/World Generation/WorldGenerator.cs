using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : DebuggableBehavior
{
    #region Variables / Properties

    public List<Biome> Biomes;

    private Biome _currentBiome;
    private BiomeGenerator _biomeGenerator;
    
    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        _biomeGenerator = GetComponent<BiomeGenerator>();
    }



    #endregion Hooks

    #region Methods

    [ContextMenu("Generate First Biome")]
    public void GenerateFirstBiome()
    {
        if(_biomeGenerator == null)
            _biomeGenerator = GetComponent<BiomeGenerator>();

        if (Biomes[0] == null)
            throw new NullReferenceException("Please specify a first pre-defined biome for testing purposes.");

        _biomeGenerator.GenerateBiome(Biomes[0].Dimensions, Biomes[0].Type);
    }

    #endregion Methods
}
