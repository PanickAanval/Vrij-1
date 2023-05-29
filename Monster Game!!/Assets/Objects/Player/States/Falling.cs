using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

partial class Player
{
    public class Falling : State<Player>
    {
        private Settings m_settings = null;

        public Falling(Settings settings)
        {
            m_settings = settings;
        }

        public override void OnEnter()
        {
            root.m_movement.vertical.acceleration = m_settings.fallAcceleration;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.Tick(root.m_input, m_settings.airSpeed, m_settings.airGrip, Time.deltaTime);

            if (root.m_movement.onGround)
            {
                SwitchToState<Walking>();
                return;
            }
        }

        [System.Serializable]
        public class Settings
        {
            public float fallAcceleration;
            public float airSpeed;
            public float airGrip;
        }
    }
}
