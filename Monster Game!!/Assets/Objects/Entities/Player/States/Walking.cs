using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

partial class Player
{
    public class Walking : State<Player>
    {
        public override void OnEnter()
        {
            root.m_movement.speed = root.m_walkSpeed;
            root.m_movement.grip = root.m_groundGrip;

            root.m_movement.gravity = 0f;
            root.m_movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.ApplyInput(root.m_input, deltaTime);

            if (!root.m_movement.onGround) { SwitchToState<Falling>(); return; }
            if (Input.GetKeyDown(KeyCode.Space)) { SwitchToState<Jumping>(); return; }
            if (Input.GetKeyDown(KeyCode.F)) { SwitchToState<Grabbing>(); return; }
        }
    }
}
