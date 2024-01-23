using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{

    // A method that creates an iNoiseFilter of any noise filter type
    public static iNoiseFilter CreateNoiseFilter(NoiseSettings settings) {
        
        switch(settings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new NoiseFilter(settings.simpleNoiseSettings);
            case NoiseSettings.FilterType.Rigid:
                return new RigidNoiseFilter(settings.rigidNoiseSettings);
        }
        return null;

    }
}
