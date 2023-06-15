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
    [Header("Entity Properties:")]
    [SerializeField] protected ExtendedMovementSettings m_moveSettings;

    [Header("Entity References:")]
    [SerializeField] protected Animator m_animator;

    #region Properties

    public ExtendedMovementSettings moveSettings { get => m_moveSettings; }

    #endregion

    //  Run-time:
    protected MovementBase m_movement   = null;
    protected FSM m_stateMachine        = null;

    public void SwitchAnimation(AnimationClip animation, float time = 0)
    {
        if (animation == null)
        {
            Debug.LogWarning($"Animation not found within entity's properties.", gameObject);
            return;
        }
        m_animator.CrossFadeInFixedTime(animation.name, time);
    }

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
        [Min(0f)]   public float jumpForce  = 10f;
        [Min(0f)]   public float airGrip    = 3f;
        [Space]
        [Min(0f)]   public float fallDrag   = 0f;
        [Min(1)]    public float fallMult   = 1f;
    }
}
