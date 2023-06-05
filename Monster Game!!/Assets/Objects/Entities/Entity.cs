using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;


public abstract class Entity : MonoBehaviour
{
    [Header("Properties:")]
    public float walkSpeed = 8f;
    public float groundGrip = 8f;
    [Space]
    public float airGrip = 5f;
    public float gravity = -9.81f;

    public virtual void DrawGizmos() { }
}
