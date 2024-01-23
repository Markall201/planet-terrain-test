using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    // variables to hold min and max values
    public float min { get; private set; }
    public float max { get; private set; }

    // Constructor
    public MinMax() {
        // default values that can be overridden
        min = float.MaxValue;
        max = float.MinValue;
    }

    //bring in a new value and see what's biggest or least
    public void addValue(float v) 
    {

        if (v > max) {
            max = v;
        }

        if (v < min) {
            min = v;
        }


    }
}
