using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{

    public Texture2D heightmap;
    
    // radius control
    public float planetRadius = 1f;

    public float heightMultiplier = 1f;
    // Noise settings - now replaced with an array of multiple layers
    public NoiseLayer[] noiseLayers;


    //Constructor
    //public ShapeSettings() {
    //    this.planetRadius = 1f;
    //    noiseLayers = new NoiseLayer[1];
    //}


    //sebastian does this so i will too
    //Each noise layer has its own settings and toggle of visibility
    [System.Serializable]
    public class NoiseLayer 
    {
        public bool enabled = true;

        // option to use first layer as a mask - so second layer terrain only appears there
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }

}
