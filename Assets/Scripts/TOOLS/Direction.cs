using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{

    none=0,

    forward = 1, /*opposite*/ backward = ~forward,

    right = 2, /*opposite*/ left = ~right,

    up = 3, /*opposite*/ down = ~up,



}
