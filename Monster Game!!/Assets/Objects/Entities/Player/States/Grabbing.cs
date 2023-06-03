using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;

partial class Player
{
    public class Grabbing : State<Player>
    {
        private Timer m_timer = null;

        public Grabbing(float grabTime)
        {
            m_timer = new Timer(grabTime);
        }

        public override void OnEnter()
        {
            root.m_grabbing.SetState(GrabbyHandler.State.Active);
            root.m_movement.grip = root.m_grabGrip;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            root.m_grabbing.Tick();

            if (!root.m_movement.onGround) { SwitchToState<Falling>(); return; }
            //if (Input.GetKeyDown(KeyCode.Space)) { SwitchToState<Jumping>(); return; }
            if (m_timer.HasReached(deltaTime)) { SwitchToState<Walking>(); return; }
        }

        public override void OnDrawGizmos()
        {
            root.m_grabbing.DrawGizmos();
        }
    }
}