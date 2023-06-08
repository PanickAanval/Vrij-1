﻿using System;
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
        [SerializeField] private float m_radius = 1f;
        [SerializeField] private LayerMask m_mask;

        //  Run-time:
        private Overlapper<IGrabbable> m_hitBox = null;
        private State m_state = State.Inactive;

        //  Reference:
        private Player m_player = null;
        private Transform m_handBone = null;

        public void Setup(Player parent)
        {
            m_player = parent;
            m_handBone = parent.m_grabPivot;
            m_hitBox = new Overlapper<IGrabbable>(m_radius, m_mask);
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
            m_hitBox.Overlap(m_handBone.position);
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
            m_hitBox.DrawGizmos(m_handBone.position, Color.red);
        }

        public enum State { Active, Inactive }
    }
}
