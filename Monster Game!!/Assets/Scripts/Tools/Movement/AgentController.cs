﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Utilities;

namespace Joeri.Tools.Movement
{
    [System.Serializable]
    public class AgentController : MovementBase
    {
        BehaviorHandler m_behaviorHandler = new BehaviorHandler();

        /// <summary>
        /// Apply the passed in behaviors to the controller's behavior handler.
        /// </summary>
        public void SetBehaviors(params Behavior[] behaviors)
        {
            m_behaviorHandler.SetBehaviors(behaviors);
        }

        /// <summary>
        /// Clears the behaviors set in the controller's behavior handler.
        /// </summary>
        public void ClearBehaviors()
        {
            m_behaviorHandler.ClearBehaviors();
        }

        public void ApplyBehaviorVelocity(float deltaTime)
        {
            var context = new Behavior.Context(deltaTime, speed, Vectors.VectorToFlat(controller.transform.position), horizontalVelocity);
            var desiredVelocity = m_behaviorHandler.GetDesiredVelocity(context);

            ApplyDesiredVelocity(desiredVelocity, deltaTime);
        }

        public override void DrawGizmos()
        {
            base.DrawGizmos();
            m_behaviorHandler.DrawGizmos(controller.transform.position);
        }
    }
}
