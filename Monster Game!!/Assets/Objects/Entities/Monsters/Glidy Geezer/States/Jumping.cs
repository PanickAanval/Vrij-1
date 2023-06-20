using System;
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
    public class Jumping : FlexState<GlidyGeezer>
    {
        public Jumping(GlidyGeezer root) : base(root) { }

        public override void OnEnter()
        {
            root.movement.velocity  = (root.transform.forward + Vector3.up) * root.m_moveSettings.jumpForce;

            root.movement.grip      = root.m_moveSettings.airGrip;
            root.movement.gravity   = root.m_moveSettings.baseGravity;
        }

        public override void OnTick(float deltaTime)
        {
            if (root.movement.verticalVelocity < 0) { SwitchToState(typeof(Falling)); return; }
            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }
    }
}
