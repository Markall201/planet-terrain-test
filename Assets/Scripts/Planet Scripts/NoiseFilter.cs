using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple Noise Filter
// Object that modifies the raw noise from the Noise script.
// This uses NoiseSettings as well to affect the noise value

public class NoiseFilter : iNoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();



    public NoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
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

        for (int i = 0; i < settings.numLayers; i++) {
            
            // noise.Evaluate(point * frequency + settings.centre) returns a value between -1 and 1, so we change that to a value between 0 and 1.
            v = noise.Evaluate(point * frequency + settings.centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
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
