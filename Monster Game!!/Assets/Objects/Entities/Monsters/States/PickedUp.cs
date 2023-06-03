using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

public abstract partial class Monster
{
    public class PickedUp : State<Monster>
    {
        //  Properties:
        private float m_followSpeed;

        //  Run-time:
        private Vector3 m_velocity;

        //  Reference:
        private Transform m_grabber;

        public void Setup(float followSpeed, Transform grabber)
        {
            m_followSpeed = followSpeed;
            m_grabber = grabber;
        }

        public override void OnTick(float deltaTime)
        {
            root.transform.position = Vector3.SmoothDamp(root.transform.position, m_grabber.position, ref m_velocity, m_followSpeed, Mathf.Infinity, deltaTime);
            root.transform.rotation = Quaternion.Euler(0f, m_grabber.eulerAngles.y, 0f);
        }
    }
}