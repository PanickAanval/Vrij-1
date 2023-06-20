using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

public partial class Player
{
    public class Launched : FlexState<Player>
    {
        public Launched(Player root) : base(root) { }

        public void Setup(float launchPower)
        {
            root.movement.verticalVelocity = launchPower;
        }

        public override void OnEnter()
        {
            root.movement.grip = root.m_moveSettings.airGrip;
            root.movement.gravity = root.m_moveSettings.baseGravity;

            root.SwitchAnimation(root.m_animations.startJump);
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
            if (root.movement.velocity.y < 0) { SwitchToState<Falling>(); return; }
        }
    }
}