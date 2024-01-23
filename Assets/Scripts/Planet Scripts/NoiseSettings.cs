using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType filterType;
    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    // for organisation and expandability, the settings are going in classes
    // this lets us distinguish between the two and not show Rigid-specific settings for other Noise types
    [System.Serializable]
    public class SimpleNoiseSettings {
        public float strength = 1;

        public Vector3 centre;

        // Layered noise will make the terrain better - mountains etc.
        [Range(1,8)]
        public int numLayers = 1;

        // roughness and base roughness correlate to frequency of bumps on the surface
        //base roughness : base frequency
        // 0 = smooth sphere
        // 1+ or below -1 = increasingly bumpy then spiky sphere
        public float baseRoughness = 1;

        // roughness = how bumpy the base roughness bumps are!
        // values of around 1.8-2.2 look good
        public float roughness = 2;

        // Multiplier for the amplitude for each layer.
        // At 0.5, it is halved for each higher layer - so less spikes.
        // 0 = sphere
        // 1 = completely bumpy thing
        // Best around 0.5

        public float persistence = 0.5f;

        // allows us to control the sphericalness of the planet - make the terrain recede into it
        public float minValue;
    }
    
    // RigidNoiseSettings inherits all SimnpleNoiseSettings and has its own settings too
    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings {
        //just for RigidNoiseFilter
        public float weightMultiplier = 0.8f;
    }
   

    
  
}
