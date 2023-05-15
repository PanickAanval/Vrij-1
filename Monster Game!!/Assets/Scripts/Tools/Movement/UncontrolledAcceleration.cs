using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class handling the properties of velocity on a singular axis, only applicable in the air.
/// </summary>
public class UncontrolledAcceleration
{
    public float acceleration   = 0f;
    public float velocity       = 0f;
    public float drag           = 0f;

    public UncontrolledAcceleration(float initialAcceleration, float initialVelocity, float initialDrag)
    {
        acceleration    = initialAcceleration;
        velocity        = initialVelocity;
        drag            = initialDrag;
    }

    /// <summary>
    /// Iterates the velocity based on the current set properties.
    /// </summary>
    public void Tick(float deltaTime)
    {
        velocity += acceleration * deltaTime;
        velocity -= drag * deltaTime;
    }

    public void AddForce(float force)
    {
        velocity += force;
    }
}
