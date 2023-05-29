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
        private Settings m_settings = null;

        public Walking(Settings settings)
        {
            m_settings = settings;
        }

        public override void OnEnter()
        {
            root.m_movement.vertical.acceleration = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.Tick(root.m_input, m_settings.speed, m_settings.grip, Time.deltaTime);

            if (!root.m_movement.onGround)
            {
                SwitchToState<Falling>();
                return;
            }
        }

        [System.Serializable]
        public class Settings
        {
            public float speed;
            public float grip;
        }
    }
}
