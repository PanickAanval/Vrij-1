﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class GlidyGeezer
{
    public class Rotating : EntityState<GlidyGeezer>
    {
        private Timer m_timer = null;

        public Rotating(GlidyGeezer root, Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            m_timer = new Timer(root.m_rotationTime);

            root.movement.canRotate         = false;
            root.movement.grip              = root.m_moveSettings.baseGrip;
            root.movement.gravity           = 0f;
            root.movement.verticalVelocity  = 0f;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            root.transform.localEulerAngles += Vector3.up * ((root.m_rotateAmount / root.m_rotationTime) * deltaTime);

            if (!root.movement.onGround)        { SwitchToState(typeof(Falling));   return; }
            if (m_timer.HasReached(deltaTime))  { SwitchToState(typeof(Jumping));   return; }
        }

        public override void OnExit()
        {
            root.movement.canRotate         = true;
            m_timer                         = null;
        }
    }
}
