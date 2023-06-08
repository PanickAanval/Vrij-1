using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

partial class Player
{
    public class Walking : FlexState<Player>
    {
        public Walking(Player root) : base(root) { }

        public override void OnEnter()
        {
            root.movement.speed = root.m_moveSettings.baseSpeed;
            root.movement.grip = root.m_moveSettings.baseGrip;
            root.movement.gravity = 0f;
            root.movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyInput(root.m_input, deltaTime);

            if (!root.movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchToState(typeof(Jumping));
                return;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (root.m_grabbingItem == null)
                {
                    SwitchToState(typeof(Grabbing));
                    return;
                }
                SwitchToState(typeof(Throwing));
                return;
            }
        }
    }
}
