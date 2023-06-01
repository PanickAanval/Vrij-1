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
    [Header("Properties")]
    [SerializeField] protected float m_walkSpeed = 8f;
    [SerializeField] protected float m_groundGrip = 8f;
    [Space]
    [SerializeField] protected float m_airGrip = 5f;
    [SerializeField] protected float m_gravity = -9.81f;

    public virtual void DrawGizmos() { }
}
