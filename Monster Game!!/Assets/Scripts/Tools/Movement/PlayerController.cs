using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Movement
{
    [System.Serializable]
    public class PlayerController : MovementBase
    {
        [Space]
        public float speed = 10f;
        public float grip = 5f;

        public void ApplyInput(Vector2 input, float deltaTime)
        {
            var velocity = Vector3.zero;
                
            //  Calculating velocity in the horizontal class.
            m_horizontal.CalculateVelocity(input, speed, grip, deltaTime);

            //  Applying calculations.
            velocity.x = m_horizontal.velocity.x;
            velocity.z = m_horizontal.velocity.y;

            //  Applying rotation, before vertical velocity gets calculated.
            controller.transform.rotation = Quaternion.LookRotation(velocity);

            //  Calculating, and applying vertical velocity.
            velocity.y = m_vertical.CalculateVelocity(deltaTime);

            //  Misc.
            controller.Move(velocity * deltaTime);
            onGround = isOnGround();
        }
    }
}