using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools;

partial class Player
{
    public class Falling : State<Player>
    {
        private Swapper<float> m_gripSwapper;

        public Settings settings { get => m_settings as Settings; }

        public Falling(Settings settings) : base(settings) { }

        public override void OnEnter()
        {
            m_gripSwapper = new Swapper<float>(root.m_movement.grip * settings.gripMultiplier);
            m_gripSwapper.Swap(ref root.m_movement.grip);

            root.m_movement.SetGravityState(true);
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.ApplyInput(root.m_input, deltaTime);

            if (root.m_movement.onGround) { OnLand(); return; }
        }

        private void OnLand()
        {
            //  Return to walking if the ground does not have any special logic.
            if (!root.m_movement.groundInfo.Contains(out IStandable[] components))
            {
                SwitchToState<Walking>();
                return;
            }

            //  If it does, execute their logic.
            for (int i = 0; i < components.Length; i++)
            {
                components[i].OnStand();
            }
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
