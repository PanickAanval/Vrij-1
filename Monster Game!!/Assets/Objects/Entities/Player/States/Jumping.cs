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
    public class Jumping : FlexState<Player>
    {
        public Jumping(Player root) : base(root) { }

        public override void OnEnter()
        {
            root.movement.grip = root.m_moveSettings.airGrip;
            root.movement.gravity = root.m_moveSettings.baseGravity;
            root.movement.verticalVelocity = root.m_moveSettings.jumpForce;

            root.SwitchAnimation(root.m_animations.startJump);
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyInput(root.m_input, deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift) && root.m_airDashAvailable)
            {
                root.m_airDashAvailable = false;
                SwitchToState<Dashing>().Setup(root.m_input);
                return;
            }
            if (root.movement.velocity.y < 0)
            {
                SwitchToState(typeof(Falling)); 
                return; 
            }
        }
    }
}