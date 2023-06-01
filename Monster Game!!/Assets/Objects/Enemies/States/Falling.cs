using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Swapper<float> m_gripSwapper;

        private Settings settings { get => m_settings as Settings; }

        public Falling(Settings settings) : base(settings) { }

        public override void OnEnter()
        {
            m_gripSwapper = new Swapper<float>(root.m_movement.grip * settings.gripMultiplier);
            m_gripSwapper.Swap(ref root.m_movement.grip);

            root.m_movement.SetGravityState(true);
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

        public override void OnExit()
        {
            m_gripSwapper.Swap(ref root.m_movement.grip);
            m_gripSwapper = null;
        }

        [System.Serializable]
        public class Settings : ISettings
        {
            public float gripMultiplier;
        }
    }
}

