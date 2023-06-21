using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools;

partial class Player
{
    public class Jumping : EntityState<Player>
    {
        public Jumping(Player root, Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            root.movement.grip = root.m_moveSettings.airGrip;
            root.movement.gravity = root.m_moveSettings.baseGravity;
            root.movement.verticalVelocity = root.m_moveSettings.jumpForce;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyInput(root.m_leftInputDir, deltaTime);

            if (root.m_input.dashButtonPressed && root.m_airDashAvailable)
            {
                root.m_airDashAvailable = false;
                SwitchToState<Dashing>().Setup(root.m_leftInputDir);
                return;
            }
            if (root.movement.velocity.y < 0) { SwitchToState(typeof(Falling)); return; }
        }
    }
}