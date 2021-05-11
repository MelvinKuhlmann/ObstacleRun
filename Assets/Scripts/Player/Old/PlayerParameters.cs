using System;
using UnityEngine;

[Serializable]
public class PlayerParameters 
{
    public Vector2 maxVelocity = new Vector2(float.MaxValue, float.MaxValue);
    public float gravity = -25f;
    public float jumpFrequency = .25f;
    public float jumpMagnitude = 16f;
    public int numberOfDoubleJumps = 1;
}
