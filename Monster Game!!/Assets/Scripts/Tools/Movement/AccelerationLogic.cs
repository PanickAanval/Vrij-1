using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;

public class AccelerationLogic : MonoBehaviour
{
    private static float m_epsilon = 0.05f;

    public class Flat
    {
        public Vector2 velocity         = Vector2.zero;
        public Vector2 desiredVelocity  = Vector2.zero;

        /// <returns>The desired 3D velocity based on the given parameters and current conditions.</returns>
        public Vector3 Get3DVelocity(Vector2 input, float speed, float grip, float deltaTime)
        {
            var flatVelocity = CalculateVelocity(input, speed, grip, deltaTime);

            return new Vector3(flatVelocity.x, 0f, flatVelocity.y);
        }

        /// <returns>The desired velocity based on the given parameters and current conditions.</returns>
        public Vector2 CalculateVelocity(Vector2 input, float speed, float grip, float deltaTime)
        {
            desiredVelocity = Vector2.ClampMagnitude(input, 1f) * speed;

            //  Calculating steering.
            var steering = desiredVelocity - velocity;
            steering *= Mathf.Clamp01(grip * deltaTime);

            //  Calculating velocity.
            velocity += steering;

            //  Halting movement if slowing down, and below epsilon.
            var desiredMagn = desiredVelocity.sqrMagnitude;
            var currentMagn = velocity.sqrMagnitude;

            if (desiredMagn < currentMagn && currentMagn < m_epsilon) velocity = Vector2.zero;

            //  Returning velocity.
            return velocity;
        }

        /// <summary>
        /// Draws the acceleration class as two flat rays.
        /// </summary>
        public void Draw(Vector3 position, Color velocityColor, Color steeringColor, float opacity = 1f)
        {
            var desiredRay = Calc.FlatToVector(desiredVelocity);
            var velocityRay = Calc.FlatToVector(velocity);

            GizmoTools.DrawRay(position + velocityRay, desiredRay, steeringColor, opacity);
            GizmoTools.DrawRay(position, velocityRay, velocityColor, opacity);
        }
    }

    public class Singular
    {
        public float velocity           = 0f;
        public float desiredVelocity    = 0f;

        /// <returns>The desired velocity based on the given parameters and current conditions.</returns>
        public float CalculateVelocity(float input, float speed, float grip, float deltaTime)
        {
            desiredVelocity = Mathf.Clamp(input, -1f, 1f) * speed;

            //  Calculating steering.
            var steering = desiredVelocity - velocity;
            steering *= Mathf.Clamp01(grip * deltaTime);

            //  Calculating velocity.
            velocity += steering;

            //  Halting movement if slowing down, and below epsilon.
            var desiredMagn = Mathf.Abs(desiredVelocity);
            var currentMagn = Mathf.Abs(velocity);

            if (desiredMagn < currentMagn && currentMagn < m_epsilon) velocity = 0f;

            //  Returning velocity.
            return velocity;
        }

        /// <summary>
        /// Draws the acceleration class as two flat rays.
        /// </summary>
        public void Draw(Vector3 position, Vector3 direction, Color velocityColor, Color steeringColor, float opacity = 1f)
        {
            var desiredRay = direction.normalized * desiredVelocity;
            var velocityRay = direction.normalized * velocity;

            GizmoTools.DrawRay(position + velocityRay, desiredRay, steeringColor, opacity);
            GizmoTools.DrawRay(position, velocityRay, velocityColor, opacity);
        }
    }
}
