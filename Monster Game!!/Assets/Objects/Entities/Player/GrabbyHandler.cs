using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;

public partial class Player
{
    [System.Serializable]
    public class GrabbyHandler
    {
        [Header("Grabbing:")]
        public float radius = 1f;
        public float offset = 1f;
        public LayerMask mask;

        [Header("Carrying:")]
        public float m_carrySmoothTime = 1f;
        public Transform grabPivot;

        //  Run-time:
        private Overlapper<IGrabbable> m_hitBox = null;
        private State m_state = State.Inactive;

        public void Setup()
        {
            m_hitBox = new Overlapper<IGrabbable>(radius, mask);
        }

        public bool CaughtSomething(out IGrabbable caughtItem)
        {
            caughtItem = null;
            if (m_state == State.Inactive) return false;

            Tick();
            if (m_hitBox.caughtTargets.Count > 0)
            {
                caughtItem = m_hitBox.caughtTargets.First().Value;
                return true;
            }
            return false;
        }

        public void Tick()
        {
            if (m_state == State.Inactive) return;
            m_hitBox.Overlap(grabPivot.position + grabPivot.forward * offset);
        }

        public void SetState(State state)
        {
            switch (state)
            {
                case State.Active:
                    m_hitBox.Activate(null, null);
                    break;

                case State.Inactive:
                    m_hitBox.Deactivate();
                    break;
            }
            m_state = state;
        }

        public void DrawGizmos()
        {
            m_hitBox.DrawGizmos(grabPivot.position + grabPivot.forward * offset, Color.red);
        }

        public enum State { Active, Inactive }
    }
}
