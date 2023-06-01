using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Movement
{
    [System.Serializable]
    public class PlayerController : MovementBase
    {
        public void ApplyInput(Vector2 input, float deltaTime)
        {
            ApplyDesiredVelocity(input * speed, deltaTime);
        }
    }
}