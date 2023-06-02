using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Debugging;

namespace Joeri.Tools
{
    [System.Serializable]
    public class Overlapper
    {
        public float radius = 1f;
        public LayerMask mask;

        private bool m_active = false;
        private Dictionary<int, Collider> m_caughtColliders = null;

        private event Action<Collider> m_onEnter = null;
        private event Action<Collider> m_onStay = null;

        public void Activate(Action<Collider> onEnter, Action<Collider> onStay)
        {
            m_onEnter = onEnter;
            m_onStay = onStay;
            m_caughtColliders = new Dictionary<int, Collider>();
            m_active = true;
        }

        public void Deactivate()
        {
            m_caughtColliders.Clear();
            m_caughtColliders = null;
            m_active = false;
        }

        public void Overlap(Vector3 position)
        {
            if (!m_active)
            {
                Debug.LogWarning("Activate the Overlapper before you start checking for colliders!..");
                return;
            }

            var colliders = Physics.OverlapSphere(position, radius, mask);

            if (colliders.Length <= 0) return;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (m_caughtColliders.ContainsKey(colliders[i].GetHashCode()))
                {
                    m_onStay?.Invoke(colliders[i]);
                }
                else
                {
                    m_caughtColliders.Add(colliders[i].GetHashCode(), colliders[i]);
                    m_onEnter?.Invoke(colliders[i]);
                }
            }
        }

        public void DrawGizmos(Vector3 pos, Color color)
        {
            if (!m_active) return;
            GizmoTools.DrawSphere(pos, radius, color, 0.75f, true, 0.5f);
        }
    }
}