using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Idle : State<SpringyFella>
    {
        private Settings settings { get => m_settings as Settings; }

        public Idle(Settings settings) : base(settings) { }

        public override void OnEnter()
        {
            root.m_movement.SetGravityState(false);
            root.m_movement.vertical.velocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            if (!root.m_movement.onGround)
            {
                SwitchToState<Falling>();
                return;
            }
            if (Vector3.Distance(root.transform.position, root.m_player.transform.position) < settings.detectionRange)
            {
                SwitchToState<Follow>();
                return;
            }

            root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(root.transform.position, settings.detectionRange, Color.red, Color.white, 0.25f);
        }

        [System.Serializable]
        public class Settings : ISettings
        {
            public float detectionRange;
        }
    }
}
