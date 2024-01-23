using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{
    // replaced with a gradient
    //public Color planetColour;

    // now replaced with biomes
    //public Gradient planetGradient;

    public Material planetMaterial;
    public Texture2D planetTexture;
    public BiomeColourSettings biomeColourSettings;

    // Biomes
    [System.Serializable]
    public class BiomeColourSettings 
    {
        public NoiseSettings noise;
        
        public float noiseOffset;
        public float noiseStrength;

        // blending
        [Range(0,1)]
        public float blendAmount;

        // all the biomes on a planet
        public Biome[] biomes;
        
        
        

        //subclass for a Biome
        [System.Serializable]
        public class Biome 
        {
            
            // now individual biomes have gradients
            public Gradient gradient;
            
            // height on the planet for the biome to start at
            [Range(0,1)]
            public float startHeight;

            // tint the biome with a solid colour
            public Color tint;
            // percentage tinted
            public float tintPercent;

        }

    }
        
}
