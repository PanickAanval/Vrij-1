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
            root.movement.ApplyInput(root.m_input, deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && root.m_airJumpAvailable)
            {
                root.m_airJumpAvailable = false;
                SwitchToState(typeof(Jumping));
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && root.m_airDashAvailable)
            {
                root.m_airDashAvailable = false;
                SwitchToState<Dashing>().Setup(root.m_input);
                return;
            }
            if (root.movement.velocity.y < 0)
            {
                SwitchToState<Falling>(); 
                return; 
            }
        }
    }
}