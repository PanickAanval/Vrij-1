using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joeri.Tools.Structure;
using Joeri.Tools;

partial class Player
{
    public class Jumping : State<Player>
    {
        public override void OnEnter()
        {
            root.m_movement.grip = root.m_airGrip;
            root.m_movement.gravity = root.m_gravity;
            root.m_movement.verticalVelocity = root.m_jumpForce;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.ApplyInput(root.m_input, deltaTime);

            if (root.m_movement.velocity.y < 0) { SwitchToState<Falling>(); return; }
        }
    }
}