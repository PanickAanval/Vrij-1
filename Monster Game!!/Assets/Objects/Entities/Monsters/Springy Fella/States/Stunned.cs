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
    public class Stunned : EntityState<SpringyFella>
    {
        private Timer m_timer = null;
        private float m_startYRot = 0f;
        private float m_endYRot = 0f;

        public Settings settings { get => GetSettings<Settings>(); }

        public Stunned(SpringyFella root, Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            m_timer = new Timer(settings.duration);
            m_startYRot = root.transform.localEulerAngles.y;
            m_endYRot = root.transform.localEulerAngles.y + settings.rotationSpeed * settings.duration;

            root.movement.canRotate = false;
            root.movement.speed = root.m_moveSettings.baseSpeed;
            root.movement.grip = root.m_moveSettings.baseGrip;
            root.movement.gravity = 0f;
            root.movement.verticalVelocity = 0f;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            var currentAngles = root.transform.localEulerAngles;

            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            currentAngles.y = Mathf.Lerp(m_startYRot, m_endYRot, settings.rotationCurve.Evaluate(m_timer.percent));
            root.transform.localEulerAngles = currentAngles;

            if (m_timer.HasReached(deltaTime))
            {
                SwitchToState(typeof(Idle));
                return;
            }
            if (!root.movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
        }

        public override void OnExit()
        {
            root.movement.canRotate = true;
            m_timer = null;
        }

        [System.Serializable]
        public class Settings : EntityState<SpringyFella>.Settings
        {
            public AnimationCurve rotationCurve;
            public float rotationSpeed;
            public float duration;
        }
    }
}
