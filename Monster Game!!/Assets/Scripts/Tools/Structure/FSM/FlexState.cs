using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joeri.Tools.Structure
{
    /// <summary>
    /// Abstract base for a state within a state machine.
    /// </summary>
    public abstract class FlexState<Root> : State
    {
        /// <summary>
        /// The settings of the state, as an abstract settings interface.
        /// </summary>
        protected ISettings m_settings { get; private set; }

        /// <summary>
        /// The root class that the state machine is harbored in.
        /// </summary>
        protected Root m_root { get; private set; }

        /// <summary>
        /// Create a new state, and pass in the state's settings.
        /// </summary>
        public FlexState(Root root, ISettings settings)
        {
            m_root = root;
            m_settings = settings;
        }

        /// <summary>
        /// Create a new state, without any required settings.
        /// </summary>
        public FlexState(Root root)
        {
            m_root = root;
            m_settings = null;
        }

        /// <summary>
        /// Switches to another state using a generic variable.
        /// </summary>
        protected State SwitchToState<State>() where State : FlexState<Root>
        {
            return machine.SwitchToState<State>();
        }

        public interface ISettings { }
    }
}