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
        private Swapper<float> m_gripSwapper;

        public Settings settings { get => m_settings as Settings; }

        public Jumping(Settings settings) : base(settings) { }

        public override void OnEnter()
        {
            m_gripSwapper = new Swapper<float>(root.m_movement.grip * settings.gripMultiplier);
            m_gripSwapper.Swap(ref root.m_movement.grip);

            root.m_movement.SetGravityState(true);
            root.m_movement.vertical.velocity = settings.jumpForce;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.ApplyInput(root.m_input, deltaTime);

            if (root.m_movement.velocity.y < 0) { SwitchToState<Falling>(); return; }
        }

        public override void OnExit()
        {
            m_gripSwapper.Swap(ref root.m_movement.grip);
            m_gripSwapper = null;
        }

        [System.Serializable]
        public class Settings : ISettings
        {
            public float jumpForce;
            public float gripMultiplier;
        }
    }
}