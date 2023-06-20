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
    public class Falling : FlexState<Player>
    {
        public Falling(Player root) : base(root) { }

        public override void OnEnter()
        {
            root.movement.grip = root.m_moveSettings.airGrip;
            root.movement.gravity = root.m_moveSettings.baseGravity * root.m_moveSettings.fallMult;

            root.SwitchAnimation(root.m_animations.falling);
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
            if (root.movement.onGround) { OnLand(); return; }
        }

        private void OnLand()
        {
            root.movement.speed = root.m_moveSettings.baseSpeed;
            root.movement.grip = root.m_moveSettings.baseGrip;
            root.movement.gravity = 0f;
            root.movement.verticalVelocity = 0f;

            root.m_airDashAvailable = true;

            //  Return to walking if the ground does not have any special logic.
            //  Ideally we would want to know if any piece of ground inherits from the IStandable interface, but I have yet to figure out how.
            if (!root.movement.groundInfo.Contains(out SpringyFella[] components))
            {
                root.SwitchAnimation(root.m_animations.jumpLand);
                SwitchToState<Walking>();
                return;
            }

            //  If it does, execute their logic.
            for (int i = 0; i < components.Length; i++)
            {
                components[i].OnStand(root);
                return;
            }
        }
    }
}
