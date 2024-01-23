using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Object to control a planet's shape

public class ShapeGenerator
{

    ShapeSettings settings;
    
    // now with array of NoiseFilters to make terrain variation
    iNoiseFilter[] noiseFilters;

    // store max and min elevation
    public MinMax elevationMinMax = new MinMax();


    // New Constructor
    public ShapeGenerator() 
    { 
    }

    // Formerly the Constructor
    public void UpdateSettings(ShapeSettings settings) 
    {
        this.settings = settings;
        
        
        noiseFilters = new iNoiseFilter[settings.noiseLayers.Length];
        
        
        for (int i = 0; i < noiseFilters.Length; i++) 
        {
            // apply the settings to each filter
            noiseFilters[i] =NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    // Adapted from Procedural Planets, most of the old code is replaced with heightmap code.
    // Thanks: Sebastian Lague and ChatGPT 
    // get a location on the planet and use the heightmap to determine its shape
    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere) {
        // we'll be manipulating the point
        Vector3 vertex = pointOnUnitSphere;

        // use our GeoMaths to convert to lat/long, then convert this to a UV vector compatible with the heightmap image
        Vector2 uv = GeoMaths.PointToCoordinate(pointOnUnitSphere).ToUV();
        // get the pixel for this point
        Color pixel = settings.heightmap.GetPixelBilinear(uv.x, uv.y);

        // Adjust the vertex height using the pixel's grayscale value
        vertex += (vertex.normalized * (pixel.grayscale)) * settings.heightMultiplier;
        // scale by planet radius
        vertex *= settings.planetRadius;
        return vertex;

        
    }
}


