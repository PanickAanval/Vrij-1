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
        private State m_state = State.Idle;

        public Walking(Player root) : base(root) { }

        public override void OnTick(float deltaTime)
        {   
            switch (m_state)
            {
                case State.Idle:
                    if (root.m_input != Vector2.zero)
                    {
                        root.SwitchAnimation(root.m_animations.startRun);
                        m_state = State.Running;
                    }
                    break;

                case State.Running:
                    if (root.m_input == Vector2.zero)
                    {
                        root.SwitchAnimation(root.m_animations.endRun, 0.15f);
                        m_state = State.Idle;
                    }
                    break;
            }

            root.movement.ApplyInput(root.m_input, deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchToState(typeof(Jumping));
                return;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SwitchToState<Dashing>().Setup(root.m_input);
                return;
            }
            if (!root.movement.onGround)
            {
                SwitchToState(typeof(Falling));
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

        public override void OnExit()
        {
            m_state = State.Idle;
        }

        private enum State { Idle, Running }
    }
}
