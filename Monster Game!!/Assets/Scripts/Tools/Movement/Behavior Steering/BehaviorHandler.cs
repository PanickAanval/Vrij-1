﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Utilities;

namespace Joeri.Tools.Movement
{
    /// <summary>
    /// Class responsible for keeping track of behaviors, and returning their desired velocities if needed.
    /// </summary>
    public class BehaviorHandler
    {
        private Behavior[] m_behaviors = null;

        /// <summary>
        /// Apply the passed in behaviors to the behavior handler.
        /// </summary>
        public void SetBehaviors(params Behavior[] behaviors)
        {
            m_behaviors = behaviors;
        }

        /// <summary>
        /// Clears the behaviors set in the behavior handler.
        /// </summary>
        public void ClearBehaviors()
        {
            m_behaviors = null;
        }

        /// <returns>The combined velocity of all behaviors, clamped by the 'speed' parameter.</returns>
        public Vector2 GetDesiredVelocity(Behavior.Context context)
        {
            var desiredVelocity = Vector2.zero;

            //  Check if the behavior handler has any behaviors at all. 
            if (Util.IsUnusableArray(m_behaviors))
            {
                Debug.LogError("Behavior handler does not have any behaviors set.");
                return desiredVelocity;
            }

            //  Add all desired velocities, and clamp them.
            for (int i = 0; i < m_behaviors.Length; i++)
            {
                desiredVelocity += m_behaviors[i].GetDesiredVelocity(context);
            }
            desiredVelocity = Vector2.ClampMagnitude(desiredVelocity, context.speed);
            return desiredVelocity;
        }

        /// <summary>
        /// Draw the gizmos of all underlying behaviors
        /// </summary>
        public void DrawGizmos(Vector3 position)
        {
            if (Util.IsUnusableArray(m_behaviors)) return;
            for (int i = 0; i < m_behaviors.Length; i++) m_behaviors[i].DrawGizmos(position);
        }
    }
}
