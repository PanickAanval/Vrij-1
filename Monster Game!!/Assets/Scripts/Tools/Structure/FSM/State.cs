using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.Structure
{
    /// <summary>
    /// Abstract base for a state within a state machine.
    /// </summary>
    public abstract class State<Root>
    {
        /// <summary>
        /// The state machine this state is a part of.
        /// </summary>
        protected FSM<Root> parent { get; private set; }

        /// <summary>
        /// The settings of the state, as an abstract settings interface.
        /// </summary>
        protected ISettings m_settings { get; private set; }

        /// <summary>
        /// The root class that the state machine is harbored in.
        /// </summary>
        protected Root root { get => parent.root; }

        /// <summary>
        /// Create a new state, and pass in the state's settings.
        /// </summary>
        public State(ISettings settings)
        {
            m_settings = settings;
        }

        /// <summary>
        /// Create a new state, without any required settings.
        /// </summary>
        public State()
        {
            m_settings = null;
        }

        /// <summary>
        /// Called whenever the finite state machine the state is in, is created.
        /// </summary>
        public virtual void Initialize(FSM<Root> parent)
        {
            this.parent = parent;
        }

        #region Local Interface

        /// <summary>
        /// Switches to another state using a generic variable.
        /// </summary>
        protected State SwitchToState<State>() where State : State<Root>
        {
            return parent.SwitchToState<State>();
        }

        /// <summary>
        /// Switches to another state using a type variable.
        /// </summary>
        protected State<Root> SwitchToState(System.Type state)
        {
            return parent.SwitchToState(state);
        }

        #endregion

        /// <summary>
        /// Update function for the state.
        /// </summary>
        public virtual void OnTick(float deltaTime) { }

        /// <summary>
        /// Called whenevet the state is entered.
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Called whenever the state is exited.
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// Functions for drawing gizmos of the state.
        /// </summary>
        public virtual void OnDrawGizmos() { }

        /// <summary>
        /// Abstract interface representing any settings the state might have.
        /// </summary>
        public interface ISettings { }
    }
}