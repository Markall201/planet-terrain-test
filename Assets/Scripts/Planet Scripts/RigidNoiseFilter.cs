using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Advanced Noise Filter for peaks and ridges

public class RigidNoiseFilter : iNoiseFilter
{
    NoiseSettings.RigidNoiseSettings settings;
    Noise noise = new Noise();



    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }


    // Method that takes in a Vector3 position and calculates the noise for that position.
    //It does more noise processing than just raw noise.
    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float v;
        float weight = 1;       // weighting - we want more detail in ridges than in valleys, so weight the noise for higher layers.

        for (int i = 0; i < settings.numLayers; i++) {
            
            // Same as the simple noise filter except we want an absolute value first, then we invert by subtracting it all from 1
            v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            
            // square to make ridges more pronounced. v will be between 0 and 1 now.
            v *= v;

            // for terrain weighting
            v *= weight;
            
            // update v for the next layer so we have more noise and detail
            weight = Mathf.Clamp01(v * settings.weightMultiplier);
            
            noiseValue += v * amplitude;
            // detail will increase with each layer (roughness > 1), and amplitude will decrease (persistence < 1)
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
            //which is how we want it
        }

        // take the higher of the min value or the terrain
        noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        return noiseValue * settings.strength;
    }
}
