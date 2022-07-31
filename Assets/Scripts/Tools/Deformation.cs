using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformation
{


    /// <summary>
    /// return a deformed vector of parameter vector 
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="deformationPercentMin"></param>
    /// <param name="deformationPercentMax"></param>
    /// <returns></returns>
    public static Vector3 Deform(Vector3 vector, float deformationPercentMin, float deformationPercentMax)
    {


        float deformationX = ((Random.value > 0.5f) ? -1 : 1)
            * vector.x * Random.Range(deformationPercentMin, deformationPercentMax) / 100f;
        float deformationY = ((Random.value > 0.5f) ? -1 : 1)
            * vector.y * Random.Range(deformationPercentMin, deformationPercentMax) / 100f;
        float deformationZ = ((Random.value > 0.5f) ? -1 : 1)
            * vector.z * Random.Range(deformationPercentMin, deformationPercentMax) / 100f;

        vector.x += deformationX;
        vector.y += deformationY;
        vector.z += deformationZ;

        return vector;

    }


    public static Vector2 Deform(Vector2 vector, float deformationPercentMin, float deformationPercentMax)
    {


        float deformationX = ((Random.value > 0.5f) ? -1 : 1)
            * vector.x * Random.Range(deformationPercentMin, deformationPercentMax) / 100f;
        float deformationY = ((Random.value > 0.5f) ? -1 : 1)
            * vector.y * Random.Range(deformationPercentMin, deformationPercentMax) / 100f;


        vector.x += deformationX;
        vector.y += deformationY;

        return vector;

    }

    /// <summary>
    /// return a deformed float value of parameter float value
    /// </summary>
    /// <param name="val"></param>
    /// <param name="deformationPercentMin"></param>
    /// <param name="deformationPercentMax"></param>
    /// <returns></returns>
    public static float Deform(float val, float deformationPercentMin, float deformationPercentMax)
    {


        float deformation = ((Random.value > 0.5f) ? -1 : 1)
            * val * Random.Range(deformationPercentMin, deformationPercentMax) / 100f;

        val += deformation;

        return val;

    }


}