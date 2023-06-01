using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Follow : State<SpringyFella>
    {
        private Settings settings { get => m_settings as Settings; }

        public Follow(Settings settings) : base(settings) { }
        
        public override void OnEnter()
        {
            root.m_movement.SetBehaviors(new Pursue(settings.lookAheadTime, root.m_player.transform));
        }

        public override void OnTick(float deltaTime)
        {
            if (!root.m_movement.onGround)
            {
                SwitchToState<Falling>();
                return;
            }
            if (Vector3.Distance(root.transform.position, root.m_player.transform.position) > settings.motivationRange)
            {
                SwitchToState<Idle>();
                return;
            }

            root.m_movement.ApplyBehaviorVelocity(deltaTime);
        }

        public override void OnExit()
        {
            root.m_movement.ClearBehaviors();
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(root.transform.position, settings.motivationRange, Color.blue, Color.white, 0.1f);
        }


        [System.Serializable]
        public class Settings : ISettings
        {
            public float lookAheadTime;
            public float motivationRange;
        }
    }
}