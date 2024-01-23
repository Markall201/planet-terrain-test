using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Interface for Noise Filters

public interface iNoiseFilter
{
    // all NoiseFilters must implement this
    float Evaluate(Vector3 point);
}
