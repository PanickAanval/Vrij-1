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
    public class Movement
    {
        public float speed;
        public float grip;

        private AccelerationLogic.Flat m_acceleration = new AccelerationLogic.Flat();

        public Vector3 velocity { get => Calc.FlatToVector(m_acceleration.velocity); }
        public Vector2 horizontalVelocity { get => m_acceleration.velocity; }

        public void Setup()
        {

        }

        public void Tick(float deltaTime)
        {

        }

}
