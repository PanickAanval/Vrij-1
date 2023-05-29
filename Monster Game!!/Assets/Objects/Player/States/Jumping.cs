using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joeri.Tools.Structure;

partial class Player
{
    public class Jumping : State<Player>
    {
        private Settings m_settings = null;

        public Jumping(Settings settings)
        {
            m_settings = settings;
        }

        public override void OnEnter()
        {
            root.m_movement.vertical.acceleration = m_settings.fallAcceleration;
            root.m_movement.vertical.velocity = m_settings.jumpForce;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.Tick(root.m_input, m_settings.airSpeed, m_settings.airGrip, deltaTime);

            if (root.m_movement.velocity.y < 0) { SwitchToState<Falling>(); return; }
        }

        [System.Serializable]
        public class Settings
        {
            public float jumpForce;
            public float fallAcceleration;
            public float airSpeed;
            public float airGrip;
        }
    }
}