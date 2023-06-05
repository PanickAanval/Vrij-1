using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

public abstract partial class Monster
{
    public class PickedUp : FlexState<Monster>
    {
        //  Properties:
        private float m_followSpeed;

        //  Run-time:
        private Vector3 m_velocity;

        //  Reference:
        private Transform m_grabber;

        public PickedUp(Monster root) : base(root) { }

        public void Setup(float followSpeed, Transform grabber)
        {
            m_followSpeed = followSpeed;
            m_grabber = grabber;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.transform.position = Vector3.SmoothDamp(m_root.transform.position, m_grabber.position, ref m_velocity, m_followSpeed, Mathf.Infinity, deltaTime);
            m_root.transform.rotation = Quaternion.Euler(0f, m_grabber.eulerAngles.y, 0f);
        }
    }
}