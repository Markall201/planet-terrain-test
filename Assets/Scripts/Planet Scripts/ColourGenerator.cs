using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Object to generate colour now that we're using shaders etc.

public class ColourGenerator
{
    ColourSettings settings;
    
    // texture settings
    Texture2D texture;
    // width
    int textureResolution = 2048;
    // height
    int textureHeight = 1024;

    iNoiseFilter biomeNoiseFilter;

    // the number of planetary biomes
    [HideInInspector]
    int numBiomes = 0;

    // New Constructor
    public ColourGenerator() 
    {
    }

    // Old Constructor, now updater
    public void UpdateSettings(ColourSettings settings) 
    {
        
        this.settings = settings;
        // pull in planetTexture settings if available
        if (settings.planetTexture != null) {
            textureResolution = settings.planetTexture.width;
            textureHeight = settings.planetTexture.height;
        }
        
        // get the number of biomes
        numBiomes = settings.biomeColourSettings.biomes.Length;

        if (texture == null || texture.height != numBiomes) {

            // create a new texture
            texture = new Texture2D(textureResolution, textureHeight);
        }
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax) 
    {
        //settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.min, elevationMinMax.max));
    
        //Debug.Log("dab");
    }

/*
    // Leftover from Procedural Planets, which had biomes. Not used anymore
    // A method calculating which biome we're (not the player) in
    // returns 0 if in first biome, 1 if in last biome
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere) 
    {
        // height on planet: 0 (south pole) to 1 (north pole)
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        
        // add noise to biome height, but make it more controllable using offset and strength values
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
        
        float biomeIndex = 0f;
        
        // how much to blend, we always want more than 0 for the Inverse Lerp
        float blendRange = settings.biomeColourSettings.blendAmount / 2f + 0.001f;

        for (int i = 0; i < numBiomes; i++) {
            // difference between heightPercent and startHeight
            float distanceDifference = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
            
            // for blending - go between + and - the blend range, this is the range of the biome gradienting from 1 to 0 basically.
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distanceDifference);
            
            // update biome index for gradient
            biomeIndex *= (1-weight);       // can reset it if weight =
            biomeIndex += i * weight;       // adds to it
        }

        
        // the function makes sure we don't return 0
        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    } */

    // Method to update the colour texture to the current planetTexture in the settings
    public void UpdateColours() {

        Texture2D planetTexture = settings.planetTexture;
        texture = planetTexture;
        
        texture.Apply();



    }

}
