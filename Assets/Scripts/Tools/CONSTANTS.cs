using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CONSTANTS
{

    /// <summary>
    /// return value between 0 and 1 that is linear with value parameter
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static float Linear(float value, float minValue, float maxValue)
    {
        if (value < minValue)
            return 0;
        if (value > maxValue)
            return 1;
        else
            return (1 / (maxValue - minValue)) * value;


    }



    /// <summary>
    /// return angle that is nonnegative and between 0-360
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static float Normalize_angle_to_pos_neg_180(float angle)
    {

        float result_angle = angle;

        while (result_angle > 180)
            result_angle -= 2 * 180;
        while (result_angle < -180)
            result_angle += 2 * 180;

        return result_angle;


    }




}