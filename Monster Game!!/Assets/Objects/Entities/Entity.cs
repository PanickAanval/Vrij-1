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
    [SerializeField] protected ExtendedMovementSettings m_moveSettings;

    #region Properties

    public ExtendedMovementSettings moveSettings { get => m_moveSettings; }

    #endregion

    //  Run-time:
    protected MovementBase m_movement   = null;
    protected FSM m_stateMachine        = null;

    public virtual void Tick(float deltaTime)
    {
        m_stateMachine.Tick(deltaTime);
    }

    protected T GetMovement<T>() where T : MovementBase
    {
        return m_movement as T;
    }

    public virtual void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position);
    }

    [System.Serializable]
    public class ExtendedMovementSettings : MovementBase.Settings
    {
        [Space]
        public float jumpForce;
        public float airGrip;
        public float fallDrag;
    }
}
