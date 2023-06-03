using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Falling : State<SpringyFella>
    {
        public override void OnEnter()
        {
            root.m_movement.grip = root.m_airGrip;
            root.m_movement.gravity = root.m_gravity;
        }

        public override void OnTick(float deltaTime)
        {
            if (root.m_movement.onGround)
            {
                SwitchToState<Idle>();
                return;
            }

            root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }
    }
}

